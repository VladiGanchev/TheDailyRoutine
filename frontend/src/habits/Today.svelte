<script>
	import { onMount } from "svelte";

	let habits = [];
	let filteredHabits = [];
	let today = "";

	let showCompleted = "all";
	let frequencyFilter = "all";

	const fetchHabits = async () => {
		const mockHabits = [
			{ id: 1, title: "Morning Jog", type: "daily", completed: false, streak: 5, isActive: true },
			{ id: 2, title: "Meditation", type: "daily", completed: true, streak: 15, isActive: true },
			{ id: 3, title: "Weekly Review", type: "weekly", completed: false, streak: 2, isActive: false },
			{ id: 4, title: "Monthly Budgeting", type: "monthly", completed: true, streak: 25, isActive: true },
		];
		return new Promise((resolve) => setTimeout(() => resolve(mockHabits), 500));
	};

	onMount(async () => {
		const date = new Date();
		today = date.toLocaleDateString("en-US", {
			weekday: "long",
			month: "long",
			day: "numeric",
			year: "numeric",
		});

		const habitsFromAPI = await fetchHabits();
		habits = habitsFromAPI.filter(habit => habit.isActive);
		filteredHabits = habits;
	});

	const toggleCompletion = habitId => {
		habits = habits.map((habit) => {
			if (habit.id === habitId) {
				habit.completed = !habit.completed;
				if (habit.completed) {
					habit.streak += 1;
					// Send put request for progress
				} else {
					habit.streak = Math.max(0, habit.streak - 1);
					// Delete progress
				}
			}
			return habit;
		});
		applyFilters();
	};

	const applyFilters = () => {
		filteredHabits = habits.filter((habit) => {
			const matchesCompleted =
				showCompleted === "all" ||
				(showCompleted === "completed" && habit.completed) ||
				(showCompleted === "not-completed" && !habit.completed);

			const matchesFrequency =
				frequencyFilter === "all" || habit.type === frequencyFilter;

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
				<option value="daily">Daily</option>
				<option value="weekly">Weekly</option>
				<option value="monthly">Monthly</option>
			</select>
		</label>
	</div>

	<div class="habit-list">
		{#if filteredHabits.length > 0}
			{#each filteredHabits as habit (habit.id)}
				<button
					class="habit-card"
					on:click={() => toggleCompletion(habit.id)}
					class:completed={habit.completed}
					style="box-shadow: {getGlowColor(habit.streak, habit.completed)};"
					aria-label="Mark habit as completed or uncompleted"
				>
					<h3 class="habit-title" class:completed={habit.completed}>
						[{habit.type.charAt(0).toUpperCase()}] {habit.title}
					</h3>
					<span class="streak">
						<span class="fire-icon">🔥</span>
						<span class="streak-number">{habit.streak}</span>
					</span>
				</button>
			{/each}
		{/if}
	</div>
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
        justify-content: space-around;
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
        box-shadow: 0 4px 16px rgba(0, 0, 0, 0.6);
        width: 100%;
        padding: 1rem;
        display: flex;
        justify-content: space-between;
        align-items: center;
        cursor: pointer;
    }

    .habit-title {
        font-size: 1.2rem;
        font-weight: bold;
    }

    .streak {
        display: flex;
        align-items: center;
        font-size: 1.2rem;
        font-weight: bold;
    }

    .fire-icon {
        margin-top: -0.3rem;
    }

    .habit-title.completed {
        text-decoration: line-through;
        color: #aaa;
    }
</style>
