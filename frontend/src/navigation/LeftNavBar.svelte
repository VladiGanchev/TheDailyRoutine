<script>
	let my_habits_selector = [
		{
			name: "My Habits",
			path: "/#/my-habits"
		},
		{
			name: "Calendar",
			path: "/#/calendar"
		},
		{
			name: "Statistics",
			path: "/#/statistics"
		}
	];

	let add_habits_selector = [
		{
			name: "Create a Habit",
			path: "/#/create-habit"
		},
		{
			name: "Pick a Habit",
			path: "/#/pick-habit"
		}
	];

	let settings = [
		{
			name: "Account",
			path: "/#/account"
		},
		{
			name: "Logout",
			path: "/#/logout"
		}
	];
</script>

<div class="nav-bar">
	<div class="profile">
		<div>The Daily Routine</div>
	</div>

	<div class="section">
		<div class="section-title">MY HABITS</div>
		{#each my_habits_selector as my_habit}
			<div class="list-item">
				<a class="item-name" href={my_habit.path}>{my_habit.name}</a>
			</div>
		{/each}
	</div>

	<div class="section">
		<div class="section-title">ADD HABITS</div>
		{#each add_habits_selector as add_habit}
			<div class="list-item">
				<a class="item-name" href={add_habit.path}>{add_habit.name}</a>
			</div>
		{/each}
	</div>

	<div class="section">
		<div class="section-title">SETTINGS</div>
		{#each settings as setting}
			<div class="list-item">
				<a class="item-name" href={setting.path}>{setting.name}</a>
			</div>
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
				font-weight: bolder;
				font-size: 1.2rem;
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

    .list-item:hover .item-name {
				color: #888;
    }

		.item-name {
				color: #fff;
		}
</style>