<script>
	let my_habits_selector = [
		{ name: "Calendar", path: "/#/calendar" },
		{ name: "My Habits", path: "/#/my-habits" },
		{ name: "Statistics", path: "/#/statistics" }
	];

	let add_habits_selector = [
		{ name: "Create a Habit", path: "/#/create-habit" },
		{ name: "Pick a Habit", path: "/#/pick-habits" }
	];

	let settings = [
		{ name: "Account", path: "/#/account" },
		{ name: "Logout", path: "/#/logout" }
	];

	export let component;
</script>

<div class="page">
	<div class="nav-bar">
		<div class="title">
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

	<div class="main-content">
		<svelte:component this={component} />
		<div class="curving-line"></div>
	</div>
</div>


<style>
    .page {
        width: 100%;
        height: 100%;
        display: flex;
        position: relative;
    }

    .nav-bar {
        width: 24vh;
        background: linear-gradient(to right, rgba(22, 22, 22, 1), rgba(36, 36, 36, 0.2) 75%);
        color: white;
        height: 100vh;
        margin: 0;
        padding: 1rem;
        display: flex;
        flex-direction: column;
        gap: 1rem;
        z-index: 2;
        opacity: 100%
    }

    .title {
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

    .main-content {
        display: flex;
        justify-content: center;
        flex-grow: 1;
        /*overflow: hidden;*/
        position: relative;
        z-index: 1;
    }

    /* Curving lines */
    .main-content::before,
    .main-content::after {
        content: "";
        position: absolute;
        top: 50%; /* Center vertically */
        left: -10%; /* Start slightly outside the left boundary */
        width: 200%; /* Extend beyond right boundary for a smooth curve */
        height: 400px;
        background: linear-gradient(
                to right,
                rgba(0, 123, 255, 0) 0%,
                rgba(0, 123, 255, 0.5) 50%,
                rgba(0, 123, 255, 0) 100%
        );
        opacity: 0.3; /* Low opacity */
        filter: blur(5px); /* Add glow effect */
        z-index: -1; /* Place behind content */
        pointer-events: none; /* Prevent interaction */
        transform: rotate(-10deg);
        border-radius: 50%;
    }

    /* First curving line */
    .main-content::before {
        animation: curve1 8s infinite alternate ease-in-out;
    }

    /* Second curving line */
    .main-content::after {
        top: 60%; /* Slightly below the first */
        animation: curve2 10s infinite alternate ease-in-out;
    }

    /* Additional third curving line */
    .main-content .curving-line {
        position: absolute;
        top: 40%; /* Slightly above others */
        left: -10%;
        width: 200%;
        height: 400px;
        background: linear-gradient(
                to right,
                rgba(0, 123, 255, 0) 0%,
                rgba(0, 123, 255, 0.5) 50%,
                rgba(0, 123, 255, 0) 100%
        );
        opacity: 0.3;
        filter: blur(5px);
        z-index: -1;
        pointer-events: none;
        transform: rotate(10deg);
        border-radius: 50%;
        animation: curve3 12s infinite alternate ease-in-out;
    }

    /* Animations for smooth movement */
    @keyframes curve1 {
        0% {
            transform: translateY(-20px) rotate(-10deg);
        }
        100% {
            transform: translateY(20px) rotate(-15deg);
        }
    }

    @keyframes curve2 {
        0% {
            transform: translateY(30px) rotate(-20deg);
        }
        100% {
            transform: translateY(-10px) rotate(-25deg);
        }
    }

    @keyframes curve3 {
        0% {
            transform: translateY(-15px) rotate(10deg);
        }
        100% {
            transform: translateY(25px) rotate(5deg);
        }
    }
</style>