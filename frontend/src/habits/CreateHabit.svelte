<script>
	let title = '';
	let description = '';
	let frequency = 'daily';
	let startNow = false;
	let predefinedHabit = false;

	let errorMessage = '';

	const handleSubmit = () => {
		// Validate
		if (!title || !description) {
			errorMessage = 'Title and Description are required.';
		} else {
			// TODO: Send request for new habit to be created
			const newHabit = {
				title,
				description,
				frequency,
				startNow,
				predefinedHabit
			};
			console.log('New Habit Created:', newHabit);
		}
	};
</script>

<div class="create-habit-form">
	<h2>Create a New Habit</h2>

	<form on:submit|preventDefault={handleSubmit}>
		<div class="form-group">
			<label for="habit-title">Title of Habit</label>
			<input
				type="text"
				id="habit-title"
				placeholder="Name your habit"
				bind:value={title}
			/>
		</div>

		<div class="form-group">
			<label for="habit-description">Description</label>
			<textarea
				id="habit-description"
				placeholder="Describe your habit"
				bind:value={description}
			></textarea>
		</div>

		<div class="form-group">
			<label for="habit-frequency">Frequency</label>
			<select id="habit-frequency" bind:value={frequency}>
				<option value="daily">Daily</option>
				<option value="weekly">Weekly</option>
				<option value="monthly">Monthly</option>
			</select>
		</div>

		<div class="form-group checkbox-group">
			<label class="start-now-label">Start Now
				<input
					type="checkbox"
					id="start-now"
					bind:checked={startNow}
				/>
				<span class="checkmark"></span>
			</label>
		</div>

		<div class="form-group checkbox-group">
			<label class="predefined-habit-label">Predefined Habit
				<input
					type="checkbox"
					id="predefined-habit"
					bind:checked={predefinedHabit}
				/>
				<span class="checkmark"></span>
			</label>
		</div>

		{#if errorMessage}
			<div class="error-message">{errorMessage}</div>
		{/if}

		<button type="submit" class="submit-btn">Create Habit</button>
	</form>
</div>

<style>
    .create-habit-form {
        background-color: #2d2d2d;
        color: white;
        padding: 1rem;
        border-radius: 8px;
        width: 32rem;
        margin: 4rem auto;
        display: flex;
        justify-content: center;
        flex-direction: column;
        align-items: center;
				max-height: 75vh;
    }

    form {
        width: 100%;
        margin: 0 2rem;
    }

    h2 {
        text-align: center;
        margin-bottom: 1.5rem;
        padding-bottom: 0.5rem;
        font-size: 1.5rem;
        border-bottom: #eee 1px solid;
    }

    .form-group {
        margin: 1rem;
    }

    label {
        font-weight: bold;
        display: block;
        margin-bottom: 0.5rem;
    }

    input, textarea, select {
        width: 100%;
        padding: 0.75rem;
        background-color: #1e1e1e;
        color: white;
        border: 1px solid #333;
        border-radius: 4px;
        font-size: 1rem;
    }

    textarea {
        height: 10rem;
        width: 100%;
        resize: none;
    }

    button.submit-btn {
        width: 100%;
        padding: 0.75rem;
        background-color: #0061f2;
        color: white;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        font-size: 1.1rem;
    }

    button.submit-btn:hover {
        background-color: #0051d1;
    }

    .checkbox-group {
        display: flex;
        align-items: center;
        justify-content: space-between;
    }

    .checkbox-group label {
        font-size: 1rem;
        display: flex;
        flex-direction: row;
        flex-grow: 1;
        justify-content: space-between;
    }

    .checkbox-group input {
        display: none;
    }

    .checkmark {
        display: block;
        width: 1.5rem;
        height: 1.5rem;
				background-color: #222;
        border: 2px solid #888;
        border-radius: 4px;
        transition: all 0.1s ease-in-out;
    }

    .checkbox-group label:hover .checkmark {
        border-color: #00bcd4;
    }

    .checkbox-group label input:checked + .checkmark {
        background-color: #00bcd4;
        border-color: #00bcd4;
    }

    .error-message {
        color: #f87171;
        background-color: #2e2e2e;
        padding: 0.5rem;
        border-radius: 5px;
        margin-bottom: 1rem;
        margin-top: -2rem;
        text-align: center;
        font-weight: bold;
    }
</style>
