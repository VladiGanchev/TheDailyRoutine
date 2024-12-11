<script>
	let my_habits_selector = ["My Habits", "Calendar", "Statistics"];
	let add_habits_selector = ["Create a Habit", "Pick a Habit"]
	let settings = ["Account", "Logout"]
</script>

<div class="nav-bar">
	<div class="profile">
		<div>The Daily Routine</div>
	</div>

	<div class="section">
		<div class="section-title">MY HABITS</div>
		{#each my_habits_selector as my_habit}
			<div class="list-item">{my_habit}</div>
		{/each}
	</div>

	<div class="section">
		<div class="section-title">ADD HABITS</div>
		{#each add_habits_selector as add_habit}
			<div class="list-item">{add_habit}</div>
		{/each}
	</div>

	<div class="section">
		<div class="section-title">SETTINGS</div>
		{#each settings as setting}
			<div class="list-item">{setting}</div>
		{/each}
	</div>

</div>

<style>
    .nav-bar {
				position: fixed;
        width: 240px;
        background-color: #1e1e1e;
        color: white;
        height: 100vh;
				margin: 0;
        padding: 1rem;
        display: flex;
        flex-direction: column;
        gap: 1rem;
    }

    .profile {
        display: flex;
        justify-content: center;
        padding: 0.5rem 0;
        border-bottom: 1px solid #333;
    }

    .section {
        margin-top: 1rem;
    }

    .section-title {
        font-weight: bold;
        margin-bottom: 0.5rem;
        color: #888;
    }

    .list-item {
        padding: 0.5rem 0;
        cursor: pointer;
        border-radius: 4px;
    }

    .list-item:hover {
        background-color: #333;
    }

    .icon {
        margin-right: 0.5rem;
    }
</style>