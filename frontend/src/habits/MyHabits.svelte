<script>
  import { navigateTo } from "../js/helpers";
    import {user} from "../js/stores";
	// Retrieve habits from localStorage directly during initialization
	import { habits, deleteHabit } from "../js/stores";

	let userHabits = $habits;

	const handleDelete = (index) => {
		deleteHabit(index); // Remove habit from the store
	};
	// Save habits back to localStorage
	const saveToLocalStorage = () => {
		localStorage.setItem("user", JSON.stringify(habits));
	};

	let editingHabit = null;

	// Function to handle editing a habit
	const handleEditHabit = (habit) => {
		editingHabit = { ...habit };
	};

	// Function to save an edited habit
	// const handleSaveHabit = () => {
	// 	const index = habits.forEa((h) => h.id === editingHabit.id);
	// 	if (index !== -1) {
	// 		habits[index] = { ...editingHabit };
	// 	}
	// 	editingHabit = null;
	// 	saveToLocalStorage(); // Update localStorage
	// };
    
	// Function to cancel editing
	const handleCancelEdit = () => {
		editingHabit = null;
	};

	// // Function to delete a habit
	// const handleDeleteHabit = (id) => {
	// 	habits = habits.filter((habit) => habit.id !== id);
	// 	saveToLocalStorage(); // Update localStorage
	// };
</script>

<div class="my-habits-container">
	<h2>My Habits</h2>

	<!-- Show habits -->
	{#if habits !== null}
		<div class="habit-cards">
			{#each userHabits as habit}
				<div class="habit-card">
					{#if editingHabit && editingHabit.id === habit.id}
						<div class="card-content">
							<input
								type="text"
								bind:value={editingHabit.title}
								placeholder="Habit Title"
								class="habit-input"
							/>
							<textarea
								bind:value={editingHabit.description}
								placeholder="Habit Description"
								class="habit-textarea"
							></textarea>
						</div>
						<div class="card-actions">
							<button class="save-btn">Save</button>
                            <!-- <button class="save-btn" on:click={handleSaveHabit}>Save</button> -->
							<button class="cancel-btn" on:click={handleCancelEdit}>Cancel</button>
						</div>
					{:else}
						<div class="card-content">
							<h3 class="habit-title">{habit.title}</h3>
							<p class="habit-description">{habit.description}</p>
						</div>
						<div class="card-actions">
							<button class="edit-btn" on:click={() => handleEditHabit(habit)}>Edit</button>
							<!-- <button class="delete-btn" on:click={() => handleDeleteHabit(habit.id)}>Delete</button> -->
						</div>
					{/if}
				</div>
			{/each}
		</div>
	{:else}
		<p class="no-habits-message">You don't have any habits yet. Add some habits first!</p>
	{/if}
</div>

<style>
	.my-habits-container {
		margin: 2rem auto;
		background-color: #2d2d2d;
		border-radius: 8px;
		color: white;
		max-width: 85%;
		text-align: center;
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

	.card-actions {
		display: flex;
		justify-content: space-between;
	}

	.edit-btn, .delete-btn, .save-btn, .cancel-btn {
		background-color: #0061f2;
		color: white;
		border: none;
		border-radius: 5px;
		padding: 0.5rem 1rem;
		margin-top: 0.5rem;
		cursor: pointer;
		transition: background-color 0.2s ease-in-out;
	}

	.edit-btn:hover, .delete-btn:hover, .save-btn:hover, .cancel-btn:hover {
		background-color: #0051d1;
	}

	.no-habits-message {
		font-size: 1.2rem;
		color: #aaa;
		margin-top: 2rem;
	}
</style>
