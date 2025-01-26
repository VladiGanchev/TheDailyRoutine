<script>
	import { fetchPost } from '../js/fetch.js';
	import { navigateTo } from '../js/helpers.js';

	let frequency = '1';
	let isPublic = false; // Нова променлива за публичния статус


	let errorMessage = '';

	export let id = -1;
	export let title;
	export let description;
	export let visability = false;

	const handleSubmit = async () => {
    // Validate
    if (!title || !description) {
        errorMessage = 'Title and Description are required.';
    } else {
        // Create habit if it doesn't exist
        if (id === -1) {
            const newHabitBody = {
                Title: title,
                Description: description,
                IsPublic: isPublic,  // Изпращаме публичния статус
            }
            const newHabitResponse = await fetchPost("/api/habits/predefined", newHabitBody);
            if (!newHabitResponse.success) {
                errorMessage = newHabitResponse.message;
                return;
            }
            id = newHabitResponse.data.habitId;
        }
			// Add habit to user
			let freq = parseInt(frequency);
			const assignedHabitBody = {
				HabitId: id,
				Frequency: freq,
				Visability: visability,
			};
			const assignedHabitResponse = await fetchPost("/api/habits/assign", assignedHabitBody);
			if (!assignedHabitResponse.success) {
				errorMessage = assignedHabitBody.message;
			} else {
				navigateTo("/my-habits")
			}
		}
	};
</script>

<div class="create-habit-form">
	<h2>Create a New Habit</h2>

	{#if errorMessage}
		<div class="error-message">{errorMessage}</div>
	{/if}

	<form on:submit|preventDefault={handleSubmit}>
		<div class="form-group">
			<label for="habit-title">Title of Habit</label>
			{#if id === -1}
				<input
					type="text"
					id="habit-title"
					placeholder="Name your habit"
					bind:value={title}
				/>
			{:else}
				<input
					type="text"
					id="habit-title"
					placeholder="Name your habit"
					bind:value={title}
					readonly
				/>
			{/if}
		</div>

		<div class="form-group">
			<label for="habit-description">Description</label>
			{#if id === -1}
				<textarea
					id="habit-description"
					placeholder="Describe your habit"
					bind:value={description}
				></textarea>
			{:else}
				<textarea
					id="habit-description"
					placeholder="Describe your habit"
					bind:value={description}
					readonly
				></textarea>
			{/if}
		</div>

		<div class="form-group">
			<label for="habit-frequency">Frequency</label>
			{#if id === -1}
				<select id="habit-frequency" bind:value={frequency}>
					<option value="1">Daily</option>
					<option value="2">Weekly</option>
					<option value="3">Monthly</option>
				</select>
			{:else}
				<select id="habit-frequency" bind:value={frequency} disabled>
					<option value="1">Daily</option>
					<option value="2">Weekly</option>
					<option value="3">Monthly</option>
				</select>
			{/if}
		</div>

		<div class="form-group">
			<label for="visability">Frequency</label>
			{#if id === -1}
				<select id="visability" bind:value={visability}>
					<option value='true'>Public</option>
					<option value='false'>Private</option>
				</select>
			{:else}
				<select id="visability" bind:value={visability} disabled>
					<option value='true'> Public</option>
					<option value='false'>Private</option>
				</select>
			{/if}
		</div>

		<button type="submit" class="submit-btn">Create Habit</button>
	</form>
</div>

<style>
    .create-habit-form {
        background-color: #2d2d2d;
        padding: 1rem;
        border-radius: 8px;
        width: 32rem;
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    form {
        width: 100%;
				display: flex;
				flex-direction: column;
    }

    h2 {
        margin: 1rem auto 1.5rem ;
        padding-bottom: 0.5rem;
        border-bottom: #eee 1px solid;
    }

    .form-group {
        margin: 0.5rem 1rem;
    }

    label {
        font-weight: bold;
        display: block;
        margin-bottom: 0.5rem;
    }

    input, textarea, select {
		color:#eee;
        width: 100%;
        padding: 0.75rem;
        background-color: #1e1e1e;
        border: 1px solid #333;
        border-radius: 4px;
        font-size: 1rem;
    }

    textarea {
        height: 10rem;
        resize: none;
    }

    button.submit-btn {
        margin: 1rem;
        padding: 0.75rem;
        background-color: #0061f2;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        font-size: 1.2rem;
    }

    button.submit-btn:hover {
        background-color: #0051d1;
    }

    .error-message {
        color: #f87171;
        background-color: #2e2e2e;
        padding: 0.5rem;
        border-radius: 5px;
        font-weight: bold;
    }
</style>
