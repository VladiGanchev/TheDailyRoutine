<script>
	import CreateHabit from './CreateHabit.svelte';

	let habits = [
		{ id: 1, title: "Morning Jog", description: "Run for 30 minutes every morning to stay fit." },
		{ id: 2, title: "Meditation", description: "Meditate for 10 minutes to calm your mind." },
		{ id: 3, title: "Read a Book", description: "Read 20 pages of any book daily." },
		{ id: 4, title: "Drink Water", description: "Drink 8 glasses of water a day to stay hydrated." },
	];

	let selectedHabit = null;
	let showCreateHabit = false;

	const handleAddHabit = (habit) => {
		selectedHabit = habit;
		showCreateHabit = true;
	};

	const handleCloseForm = () => {
		showCreateHabit = false;
	};
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
		<CreateHabit
			title={selectedHabit.title}
			description={selectedHabit.description}
			on:closeForm={handleCloseForm}
		/>
	{/if}
</div>

<style>
    .habit-list-container {
        margin: 2rem auto;
        background-color: #2d2d2d;
        border-radius: 8px;
        color: white;
				max-width: 85%;
    }

    h2 {
        font-size: 1.8rem;
        margin: 1rem 0;
        border-bottom: 1px solid #444;
        padding-bottom: 0.5rem;
				text-align: center;
    }

    .habit-cards {
        display: flex;
        flex-wrap: wrap;
				padding: 1rem;
				max-width: 100%;
        justify-content: center;
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
				margin: 1rem;
    }

    .habit-card:hover {
        transform: translateY(-5px);
        border-color: #00bcd4;
    }

    .card-content {
        text-align: left;
    }

    .habit-title {
        font-size: 1.3rem;
        margin-bottom: 0.5rem;
        font-weight: bold;
    }

    .habit-description {
        font-size: 0.95rem;
        color: #aaa;
    }

    .add-btn {
        background-color: #0061f2;
        color: white;
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
