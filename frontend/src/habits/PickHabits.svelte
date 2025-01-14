<script>
	import CreateHabit from './CreateHabit.svelte';
	import { onMount } from 'svelte';
	import { fetchGet } from '../js/fetch.js';

	let habits = [];

	let selectedHabit = null;
	let showCreateHabit = false;

	const handleAddHabit = habit => {
		selectedHabit = habit;
		showCreateHabit = true;
	};

	const handleCloseForm = () => {
		showCreateHabit = false;
	};

	onMount(async () => {
		const response = await fetchGet("/api/habits/predefined");
		habits = response.data || [];
		console.log(habits);
	});

</script>

<div class="habit-list-container">
	{#if !showCreateHabit}
		<h2>Available Habits</h2>

		<div class="habit-cards">
			{#each habits as habit}
				<div class="habit-card">
					<div class="card-content">
						<h3 class="habit-title">{habit.title}</h3>
						<p class="habit-description">{habit.description}</p>
					</div>
					<button
						class="add-btn"
						title="Add Habit"
						on:click={() => handleAddHabit(habit)}
					>
						+
					</button>
				</div>
			{/each}
		</div>
	{/if}

	{#if showCreateHabit}
		<button class="go-back-btn" on:click={handleCloseForm}>Go Back</button>
		<CreateHabit
			id={selectedHabit.id}
			title={selectedHabit.title}
			description={selectedHabit.description}
			on:closeForm={handleCloseForm}
		/>
	{/if}
</div>

<style>
    .habit-list-container {
        background-color: #2d2d2d;
        border-radius: 8px;
        max-width: 85%;
        margin: 0 auto;
        overflow: hidden;
        padding: 1rem;
    }

    .go-back-btn {
        background-color: #2d2d2d;
        color: #fff;
        font-size: 1rem;
        border: 1px solid #1e1e1e;
        border-radius: 8px;
        padding: 0.5rem 1rem;
        cursor: pointer;
        transition: background-color 0.2s ease-in-out, transform 0.2s ease-in-out;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.2);
    }

    .go-back-btn:hover {
        transform: translateY(-1px);
    }

    h2 {
        font-size: 1.8rem;
        margin-top: 1rem;
        border-bottom: 1px solid #444;
        padding-bottom: 0.5rem;
        text-align: center;
    }

    .habit-cards {
        display: flex;
        flex-wrap: wrap;
        justify-content: center;
        gap: 1rem;
        max-width: 100%;
        overflow-y: auto;
        max-height: 70vh;
        padding: 1rem;
        scrollbar-width: thin;
        scrollbar-color: #444 #2d2d2d;
    }

    .habit-cards::-webkit-scrollbar {
        width: 8px;
    }

    .habit-cards::-webkit-scrollbar-thumb {
        background-color: #444;
        border-radius: 4px;
    }

    .habit-cards::-webkit-scrollbar-track {
        background-color: #2d2d2d;
    }

    .habit-card {
        background-color: #1e1e1e;
        border: 1px solid #333;
        border-radius: 8px;
        width: 18rem;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.3);
        transition: transform 0.2s ease-in-out;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        padding: 1rem;
        margin: 0;
    }

    .habit-card:hover {
        transform: translateY(-2px);
        border-color: #00bcd4;
    }

    .habit-title {
        font-size: 1.3rem;
        margin-bottom: 0.5rem;
        font-weight: bold;
    }

    .habit-description {
        font-size: 0.9rem;
        color: #aaa;
    }

    .add-btn {
        background-color: #0061f2;
        font-size: 1.5rem;
        border: none;
        border-radius: 50%;
        width: 2.5rem;
        height: 2.5rem;
        margin: 1rem auto 0;
        cursor: pointer;
        transition: background-color 0.2s ease-in-out;
    }

    .add-btn:hover {
        background-color: #0051d1;
    }
</style>
