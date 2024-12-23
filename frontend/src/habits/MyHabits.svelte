<script>
	let myHabits = [
		{ id: 1, title: "Morning Jog", description: "Run for 30 minutes every morning to stay fit." },
		{ id: 2, title: "Meditation", description: "Meditate for 10 minutes to calm your mind." }
	];

	let editingHabit = null;
	let editTitle = '';
	let editDescription = '';

	const removeHabit = (id) => {
		myHabits = myHabits.filter((habit) => habit.id !== id);
	};

	const startEditing = (habit) => {
		editingHabit = habit.id;
		editTitle = habit.title;
		editDescription = habit.description;
	};

	const saveEdit = (id) => {
		myHabits = myHabits.map(habit => 
			habit.id === id 
				? { ...habit, title: editTitle, description: editDescription }
				: habit
		);
		editingHabit = null;
	};

	const cancelEdit = () => {
		editingHabit = null;
	};
</script>

<div class="habit-list-container">
	<h2>My Habits</h2>

	{#if myHabits.length > 0}
		<div class="habit-cards">
			{#each myHabits as habit}
				<div class="habit-card">
					{#if editingHabit === habit.id}
						<div class="edit-form">
							<input
								type="text"
								bind:value={editTitle}
								class="edit-input"
								placeholder="Habit title"
							/>
							<textarea
								bind:value={editDescription}
								class="edit-input"
								placeholder="Habit description"
							></textarea>
							<div class="edit-buttons">
								<button
									class="save-btn"
									on:click={() => saveEdit(habit.id)}
								>
									Save
								</button>
								<button
									class="cancel-btn"
									on:click={cancelEdit}
								>
									Cancel
								</button>
							</div>
						</div>
					{:else}
						<div class="card-content">
							<h3 class="habit-title">{habit.title}</h3>
							<p class="habit-description">{habit.description}</p>
						</div>
						<div class="card-actions">
							<button
								class="edit-btn"
								title="Edit Habit"
								on:click={() => startEditing(habit)}
							>
								✎
							</button>
							<button
								class="remove-btn"
								title="Remove Habit"
								on:click={() => removeHabit(habit.id)}
							>
								×
							</button>
						</div>
					{/if}
				</div>
			{/each}
		</div>
	{:else}
		<p class="no-habits">You haven't added any habits yet.</p>
	{/if}
</div>

<style>
	.habit-list-container {
		background-color: #2d2d2d;
		border-radius: 8px;
		max-width: 85%;
		padding: 1rem;
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

	.habit-title {
		font-size: 1.3rem;
		margin-bottom: 0.5rem;
		font-weight: bold;
	}

	.habit-description {
		font-size: 0.9rem;
		color: #aaa;
	}

	.card-actions {
		display: flex;
		justify-content: center;
		gap: 0.5rem;
		margin-top: 1rem;
	}

	.remove-btn, .edit-btn {
		background-color: #f44336;
		font-size: 1.5rem;
		border: none;
		border-radius: 50%;
		width: 2.5rem;
		height: 2.5rem;
		cursor: pointer;
		transition: background-color 0.2s ease-in-out;
	}

	.edit-btn {
		background-color: #2196F3;
	}

	.edit-btn:hover {
		background-color: #1976D2;
	}

	.remove-btn:hover {
		background-color: #d32f2f;
	}

	.no-habits {
		color: #aaa;
		text-align: center;
		margin-top: 1rem;
		font-size: 1.1rem;
	}

	.edit-form {
		width: 100%;
	}

	.edit-input {
		width: 100%;
		padding: 0.5rem;
		margin-bottom: 0.5rem;
		background-color: #333;
		border: 1px solid #444;
		border-radius: 4px;
		color: #fff;
	}

	textarea.edit-input {
		min-height: 80px;
		resize: vertical;
	}

	.edit-buttons {
		display: flex;
		gap: 0.5rem;
		justify-content: center;
	}

	.save-btn, .cancel-btn {
		padding: 0.5rem 1rem;
		border: none;
		border-radius: 4px;
		cursor: pointer;
		font-weight: bold;
		transition: background-color 0.2s ease-in-out;
	}

	.save-btn {
		background-color: #4CAF50;
		color: white;
	}

	.save-btn:hover {
		background-color: #388E3C;
	}

	.cancel-btn {
		background-color: #9E9E9E;
		color: white;
	}

	.cancel-btn:hover {
		background-color: #757575;
	}
</style>