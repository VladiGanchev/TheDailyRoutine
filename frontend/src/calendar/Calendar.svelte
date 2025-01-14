<script>
	import { onMount } from "svelte";
	import Today from "./Today.svelte";
	import { fetchGet } from '../js/fetch.js'; // Import the Today component

	let history = [];
	let currentDate = new Date();
	let daysInMonth = [];
	let monthTitle = "";
	let selectedDate = null; // Track the selected date
	let selectedHabits = []; // Habits associated with the selected day
	let showCalendar = true; // Flag to toggle between calendar and "Today" view

	const weekdays = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"];

	onMount(async () => {
		await getMonthlyHistory();
		updateCalendar(currentDate);
	});

	const updateCalendar = (date) => {
		monthTitle = date.toLocaleDateString("en-US", { month: "long", year: "numeric" });
		daysInMonth = getDaysForMonth(date);
	};

	const getMonthlyHistory = async () => {
		const date = new Date();
		const startDate = new Date(date.getFullYear(), date.getMonth(), 1).toISOString();
		const endDate = new Date(date.getFullYear(), date.getMonth() + 1, 0).toISOString();
		const historyResponse = await fetchGet(`/api/completions/history?startDate=${startDate}&endDate=${endDate}`);
		if (historyResponse.success) {
			history = historyResponse.data || [];
		} else {
			console.log(historyResponse.message);
		}
	}

	const getDaysForMonth = (date) => {
		const firstDayOfMonth = new Date(date.getFullYear(), date.getMonth(), 1);
		const lastDayOfMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0);
		const days = [];

		// Determine the day of the week the month starts on (Monday-based)
		const startDay = (firstDayOfMonth.getDay() + 6) % 7;

		// Fill in the days of the current month
		for (let i = 1; i <= lastDayOfMonth.getDate(); i++) {
			days.push({
				date: i,
				isCurrentMonth: true,
				completedHabitsCount: getCompletedHabitsCountForDate(i),
			});
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

		return rows;
	};

	const getCompletedHabitsCountForDate = (day) => {
		let count = 0;
		history.forEach((habitCompletion) => {
			const completed = habitCompletion.completed;
			const date = new Date(Date.parse(habitCompletion.completedAt)).getDate();
			if (completed && date === day) {
				count++;
			}
		});
		return count;
	};

	const goToPreviousMonth = () => {
		currentDate = new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, 1);
		updateCalendar(currentDate);
	};

	const goToNextMonth = () => {
		currentDate = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 1);
		updateCalendar(currentDate);
	};

	const handleDayClick = (day) => {
		selectedDate = new Date(`${currentDate.getFullYear()}-${currentDate.getMonth() + 1}-${day.date}`);
		console.log(selectedDate);
		showCalendar = false; // Hide the calendar and show the "Today" component
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
						<div
							class="calendar-cell {day.isCurrentMonth ? '' : 'empty'}"
							on:click={() => handleDayClick(day)}
						>
							{#if day.isCurrentMonth}
								<div class="date">{day.date}</div>
								<div class="completed-habits">
									{day.completedHabitsCount} Completed
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
	<div class="date-view">
		<button on:click={goBackToCalendar}>Back to Calendar</button>
		<Today date={new Date(selectedDate)} habits={selectedHabits} />
	</div>
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

    .completed-habits {
        font-size: 1rem;
        color: #28a745;
        font-weight: bold;
    }
</style>
