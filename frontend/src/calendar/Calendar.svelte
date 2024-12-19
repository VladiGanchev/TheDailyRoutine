<script>
	import { onMount } from "svelte";
	import Today from "./Today.svelte"; // Import the Today component

	let currentMonth = new Date();
	let daysInMonth = [];
	let monthTitle = "";
	let selectedDate = null; // Track the selected date
	let selectedHabits = []; // Habits associated with the selected day
	let showCalendar = true; // Flag to toggle between calendar and "Today" view

	const weekdays = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"];

	onMount(() => {
		updateCalendar(currentMonth);
	});

	const updateCalendar = (date) => {
		monthTitle = date.toLocaleDateString("en-US", { month: "long", year: "numeric" });
		daysInMonth = getDaysForMonth(date);
	};

	const getDaysForMonth = (date) => {
		const firstDayOfMonth = new Date(date.getFullYear(), date.getMonth(), 1);
		const lastDayOfMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0);
		const days = [];

		// Determine the day of the week the month starts on (Monday-based)
		const startDay = (firstDayOfMonth.getDay() + 6) % 7;

		// Fill in the days of the current month
		for (let i = 1; i <= lastDayOfMonth.getDate(); i++) {
			days.push({ date: i, isCurrentMonth: true });
		}

		// Add padding for days before the start of the month
		for (let i = 0; i < startDay; i++) {
			days.unshift({ date: null, isCurrentMonth: false });
		}

		// Group days into rows (weeks)
		const rows = [];
		let row = [];
		for (let i = 0; i < days.length; i++) {
			row.push(days[i]);
			if (row.length === 7) {
				rows.push(row);
				row = [];
			}
		}
		if (row.length > 0) {
			rows.push(row); // Push any remaining incomplete row
		}

		// If there are 6 rows, move the last row to the start of the first row
		if (rows.length === 6) {
			const lastRow = rows.pop();
			const firstRow = rows[0];

			// Take the last few days (30, 31) and merge them with the first row
			const lastDays = lastRow.filter(day => day.date !== null);

			if (lastDays.length > 0) {
				lastDays.forEach((day, index) => {
					rows[0][index] = day;
				})
			}
		}

		return rows;
	};

	const goToPreviousMonth = () => {
		currentMonth = new Date(currentMonth.getFullYear(), currentMonth.getMonth() - 1, 1);
		updateCalendar(currentMonth);
	};

	const goToNextMonth = () => {
		currentMonth = new Date(currentMonth.getFullYear(), currentMonth.getMonth() + 1, 1);
		updateCalendar(currentMonth);
	};

	const handleDayClick = (day) => {
		selectedDate = `${currentMonth.getFullYear()}-${currentMonth.getMonth() + 1}-${day.date}`;
		selectedHabits = getHabitsForDate(day.date); // Replace with actual habit data if available
		showCalendar = false; // Hide the calendar and show the "Today" component
	};

	const getHabitsForDate = (day) => {
		// For demonstration, return some static habits. Replace with your actual logic for habits.
		return [
			`Habit 1 for day ${day}`,
			`Habit 2 for day ${day}`,
		];
	};

	const goBackToCalendar = () => {
		showCalendar = true; // Show the calendar again
	};
</script>

{#if showCalendar}
	<!-- Calendar view -->
	<div class="calendar">
		<header>
			<button on:click={goToPreviousMonth}>❮</button>
			<h2>{monthTitle}</h2>
			<button on:click={goToNextMonth}>❯</button>
		</header>

		<div class="weekday-header">
			{#each weekdays as weekday}
				<div class="weekday">{weekday}</div>
			{/each}
		</div>

		<div class="calendar-grid">
			{#each daysInMonth as row}
				<div class="calendar-row">
					{#each row as day}
						<div class="calendar-cell {day.isCurrentMonth ? '' : 'empty'}" on:click={() => handleDayClick(day)}>
							{#if day.isCurrentMonth}
								<div class="date">{day.date}</div>
								<div class="habit-indicators">
									<div class="habit-box" style="background-color: red;"></div>
									<div class="habit-box" style="background-color: green;"></div>
								</div>
							{/if}
						</div>
					{/each}
				</div>
			{/each}
		</div>
	</div>
{:else}
	<!-- Today view -->
	<Today selectedDate={selectedDate} habits={selectedHabits} />
	<button on:click={goBackToCalendar}>Back to Calendar</button>
{/if}

<style>
    .calendar {
        font-family: Arial, sans-serif;
        color: white;
        background-color: #2d2d2d;
        padding: 1rem;
    }

    header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 1rem;
    }

    button {
        background-color: #444;
        color: white;
        border: none;
        border-radius: 4px;
        padding: 0.5rem 1rem;
        cursor: pointer;
    }

    h2 {
        margin: 0;
        font-size: 1.5rem;
        color: white;
    }

    .weekday-header {
        display: grid;
        grid-template-columns: repeat(7, 1fr);
        margin-bottom: 0.5rem;
        text-align: center;
    }

    .weekday {
        font-weight: bold;
        color: #aaa;
        font-size: 1.2rem;
    }

    .calendar-grid {
        display: flex;
        flex-direction: column;
    }

    .calendar-row {
        display: grid;
        grid-template-columns: repeat(7, 1fr);
        gap: 0.5rem;
    }

    .calendar-cell {
        background-color: #1e1e1e;
        border: 1px solid #444;
        border-radius: 8px;
        text-align: center;
        cursor: pointer;
        width: 100%;
        height: 90px;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: space-between;
        font-size: 1.4rem;
        padding: 0.5rem;
    }

    .calendar-cell.empty {
        background-color: transparent;
        border: none;
        cursor: default;
    }

    .date {
        font-size: 1.8rem;
        font-weight: bold;
    }

    .habit-indicators {
        display: flex;
        gap: 0.3rem;
        justify-content: center;
    }

    .habit-box {
        width: 20px;
        height: 20px;
        border-radius: 3px;
    }
</style>
