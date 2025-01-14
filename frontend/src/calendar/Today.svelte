<script>
	import { onMount } from "svelte";
	import { fetchGet, fetchPost } from '../js/fetch.js';

	let loading = true;

	let filteredHabits = [];
	let today = "";

	let showCompleted = "all";
	let frequencyFilter = "all";

	const todayDate = new Date();
	export let date = new Date();
	export let habits = [];

	let showNotesInput = null;
	let notes = "";

	const fetchToday = async () => {
		const todayHabitsResponse = await fetchGet("/api/habits/today");
		if (todayHabitsResponse.success) {
			habits = todayHabitsResponse.data.habitCompletions || [];
		} else {
			console.log(todayHabitsResponse.message);
		}
		loading = false;
	}

	const fetchDate = async (date) => {
		const queryParamDate = new Date(date - new Date().getTimezoneOffset() * 60000).toISOString()
		const historyResponse = await fetchGet(`/api/completions/history?startDate=${queryParamDate}&endDate=${queryParamDate}`);
		if (historyResponse.success) {
			habits = historyResponse.data || [];
			for(let i = 0; i < habits.length; i++) {
				habits[i].habitId = habits[i]['id'];
				delete habits[i].id;
			}
			console.log(habits);
		} else {
			console.log(historyResponse.message);
		}
		loading = false;
	}

	onMount(async () => {
		today = date.toLocaleDateString("en-US", {
			weekday: "long",
			month: "long",
			day: "numeric",
			year: "numeric",
		});

		// Fetch user's habits
		if (date.getDate() === todayDate.getDate()) {
			// Today
			await fetchToday();
		} else {
			// Any other date
			await fetchDate(date);
		}

		// Filter habits
		applyFilters();
	});

	const markAsCompleted = async (habitId) => {
		const markAsCompletedResponse = await fetchPost(`/api/habits/complete/${habitId}`, notes);
		if (markAsCompletedResponse.success) {
			await fetchToday();
			applyFilters();
		} else {
			console.log(markAsCompletedResponse.message);
		}
		notes = "";
		showNotesInput = null;
	}

	const markAsUncompleted = async (habitId) => {
		const markAsUncompletedResponse = await fetchPost(`/api/completions/incomplete/${habitId}`, {});
		if (markAsUncompletedResponse.success) {
			await fetchToday();
			applyFilters();
		} else {
			console.log(markAsUncompletedResponse.message);
		}
	}

	const applyFilters = () => {
		filteredHabits = habits.filter((habit) => {
			const matchesCompleted =
				showCompleted === "all" ||
				(showCompleted === "completed" && habit.isCompleted) ||
				(showCompleted === "not-completed" && !habit.isCompleted);

			const matchesFrequency =
				frequencyFilter === "all" || habit.frequency === Number(frequencyFilter);

			return matchesCompleted && matchesFrequency;
		});
	};

	const getGlowColor = (streak, completed) => {
		// Increase glow size if completed
		if (completed) {
			if (streak >= 16) return "gold 0 0 15px";
			if (streak >= 6) return "green 0 0 15px";
			return "blue 0 0 15px";
		} else {
			if (streak >= 16) return "gold 0 0 2px";
			if (streak >= 6) return "green 0 0 2px";
			return "blue 0 0 2px";
		}
	};
</script>

<div class="today-page">
	<div class="date-header">
		<h1>{today}</h1>
	</div>

	{#if loading}
		<div class="loading">Loading habits...</div>
	{:else if habits.length === 0 && date.getDate() === todayDate.getDate()}
		<div class="no-habits">
			<p>You haven't added any habits yet.</p>
			<a href="#/pick-habits" class="add-btn">Browse Habits</a>
		</div>
	{:else if habits.length === 0 && date.getDate() > todayDate.getDate()}
		<div class="no-habits">
			<p>You haven't completed your habits for this day. Come back later to complete them.
		</div>
	{:else if habits.length === 0 && date.getDate() < todayDate.getDate()}
		<div class="no-habits">
			<p>You haven't completed your habits for this day.
		</div>
	{:else}
		<div class="filters">
			<label>
				Show:
				<select bind:value={showCompleted} on:change={applyFilters}>
					<option value="all">All</option>
					<option value="completed">Completed</option>
					<option value="not-completed">Not Completed</option>
				</select>
			</label>

			<label>
				Frequency:
				<select bind:value={frequencyFilter} on:change={applyFilters}>
					<option value="all">All</option>
					<option value="1">Daily</option>
					<option value="2">Weekly</option>
					<option value="3">Monthly</option>
				</select>
			</label>
		</div>

		<div class="habit-list">
			{#if filteredHabits.length > 0}
				{#each filteredHabits as habit (habit.habitId)}
					<div
						class="habit-card"
						style="box-shadow: {getGlowColor(habit.streak, habit.isCompleted)};"
					>
						<div class="habit-card-section">
							<h3 class="habit-title">{habit.title}</h3>

							<div class="habit-actions">
								{#if habit.isCompleted}
									<button
										class="mark-incomplete-btn"
										on:click={() => markAsUncompleted(habit.habitId)}
									>
										✖
									</button>
								{:else}
									<button
										class="mark-complete-btn"
										on:click={() => showNotesInput = habit.habitId}
									>
										✔
									</button>
								{/if}
							</div>
						</div>

						{#if habit.isCompleted}
							<div class="notes-section">
								<p>Notes: {habit.notes || "No notes provided"}</p>
							</div>
						{/if}

						<!-- Notes Section will appear below the title when completing a habit -->
						{#if showNotesInput === habit.habitId}
							<div class="notes-section">
								<input
									bind:value={notes}
									placeholder="Add notes (optional)..."
								/>
								<div class="notes-actions">
									<button
										class="save-notes-btn"
										on:click={() => markAsCompleted(habit.habitId)}
									>
										Save
									</button>
									<button
										class="cancel-notes-btn"
										on:click={() => (showNotesInput = null)}
									>
										Cancel
									</button>
								</div>
							</div>
						{/if}
					</div>
				{/each}
			{/if}
		</div>
	{/if}
</div>

<style>
    .today-page {
        background-color: #2d2d2d;
        color: white;
        padding: 2rem;
		border-radius: 8px;
    }

    .date-header {
        margin-bottom: 2rem;
    }

    h1 {
        font-size: 2rem;
        margin: 0;
    }

    .filters {
        display: flex;
        justify-content: space-between;
        margin-bottom: 2rem;
    }

    .filters label {
        color: #aaa;
    }

    select {
        background-color: #1e1e1e;
        border: 1px solid #333;
        border-radius: 4px;
        padding: 0.4rem;
    }

    .habit-list {
        display: flex;
        flex-direction: column;
        gap: 1rem;
    }

    .habit-card {
        background-color: #1e1e1e;
        border: 1px solid #333;
        border-radius: 8px;
        padding: 1rem;
        display: flex;
		flex-direction: column;
        align-items: center;
    }

    .habit-card-section, .notes-section {
        margin: .5rem;
        display: flex;
        justify-content: space-between;
        gap: 0.5rem;
        width: 100%;
    }

	.notes-section {
		color: #ddd;
	}

    .habit-title {
        font-size: 1.2rem;
    }

    .habit-actions button {
        padding: 0.5rem 1rem;
        border: none;
        border-radius: 4px;
        cursor: pointer;
		color: white;
    }

    .mark-complete-btn {
        background-color: #28a745;
		font-size: 1rem;
    }

    .mark-incomplete-btn {
        background-color: #dc3545;
        font-size: 1rem;
    }

    input {
        width: 100%;
        padding: 0.5rem;
        border-radius: 4px;
        border: 1px solid #ccc;
    }

    .notes-actions {
        display: flex;
        gap: 0.5rem;
    }

    .save-notes-btn {
        background-color: #0069d9;
        color: white;
        padding: 0.5rem 1rem;
        border-radius: 4px;
    }

    .cancel-notes-btn {
        background-color: #6c757d;
        color: white;
        padding: 0.5rem 1rem;
        border-radius: 4px;
    }
</style>
