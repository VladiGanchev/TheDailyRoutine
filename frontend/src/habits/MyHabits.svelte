<script>
    import { onMount } from 'svelte';
    import { fetchGet, fetchDelete } from '../js/fetch.js';
    import { navigateTo } from '../js/helpers.js';

    let habits = [];
    let loading = true;
    let error = null;

    onMount(async () => {
        try {
            const response = await fetchGet('/api/habits/my');
            habits = response.data || [];
            console.log(habits);
            loading = false;
        } catch (err) {
            error = 'Failed to load habits';
            loading = false;
        }
    });

    const removeHabit = async (habitId) => {
        if (confirm('Are you sure you want to remove this habit?')) {
            try {
                await fetchDelete(`/api/habits/${habitId}`);
                habits = habits.filter(h => h.habitId !== habitId);
            } catch (err) {
                error = 'Failed to remove habit';
            }
        }
    };

    const getGlowColor = (streak, completed) => {
        if (completed) {
            if (streak >= 16) return "gold 0 0 15px";
            if (streak >= 6) return "green 0 0 15px";
            return "blue 0 0 15px";
        } else {
            if (streak >= 16) return "gold 0 0 2px";
            if (streak >= 6) return "green 0 0 2px";
            return "blue 0 0 2px";
        }
    };

    const formatDate = (date) => {
        return new Date(date).toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'short',
            day: 'numeric'
        });
    };
</script>

<div class="habits-container">
    <h1>My Habits</h1>

    {#if error}
        <div class="error-message">{error}</div>
    {/if}

    {#if loading}
        <div class="loading">Loading habits...</div>
    {:else if habits.length === 0}
        <div class="no-habits">
            <p>You haven't added any habits yet.</p>
            <a href="/habits/pick" class="add-btn">Browse Habits</a>
        </div>
    {:else}
        <div class="habits-grid">
            {#each habits as habit}
                <div class="habit-card" style="box-shadow: {getGlowColor(habit.currentStreak, false)};">
                    <div class="habit-header">
                        <h3>{habit.title}</h3>
                    </div>

                    <p>Is Public: {habit.IsPublic}</p>

                    <p class="description">{habit.description}</p>

                    <div class="stats">
                        <div class="stat">
                            <span class="label">Frequency:</span>
                            <span class="value">{habit.frequency === 1 ? 'Daily' :
                                habit.frequency === 7 ? 'Weekly' : 'Monthly'}</span>
                        </div>
                        <div class="stat">
                            <span class="label">Current Streak:</span>
                            <span class="value">🔥 {habit.currentStreak} days</span>
                        </div>
                        <div class="stat">
                            <span class="label">Best Streak:</span>
                            <span class="value">⭐ {habit.bestStreak} days</span>
                        </div>
                        <div class="stat">
                            <span class="label">Completion Rate:</span>
                            <span class="value">{habit.completionRate.toFixed(1)}%</span>
                        </div>
                    </div>

                    {#if habit.recentCompletions && habit.recentCompletions.length > 0}
                        <div class="completions">
                            <h4>Recent Activity</h4>
                            <div class="completion-list">
                                {#each habit.recentCompletions as completion}
                                    <div class="completion-item">
                            <span class="completion-date">
                                {formatDate(completion.completedAt)}
                            </span>
                                        <span class="completion-status" class:completed={completion.completed}>
                                {completion.completed ? '✓' : '×'}
                            </span>
                                        {#if completion.notes}
                                            <span class="completion-notes">{completion.notes}</span>
                                        {/if}
                                    </div>
                                {/each}
                            </div>
                        </div>
                    {/if}

                    <div class="habit-actions">
                        <button
                            class="edit-btn"
                            on:click={() => navigateTo(`/edit-habit/${habit.habitId}`)}
                        >
                            Edit
                        </button>
                        <button
                            class="remove-btn"
                            on:click={() => removeHabit(habit.habitId)}
                        >
                            Remove
                        </button>
                    </div>
                </div>
            {/each}
        </div>
    {/if}
</div>

<style>
    .habits-container {
        overflow-y: auto;
        height: 100vh;
        scrollbar-width: none;
        -ms-overflow-style: none;
    }

    .habits-container::-webkit-scrollbar {
        display: none;
    }

    .habits-grid {
        display: flex;
        flex-wrap: wrap;
        flex-direction: row;
        justify-content: center;
        align-items: center;
        gap: 2rem;
        overflow-y: auto;
    }

    h1 {
        font-size: 2rem;
        margin: 2rem 0;
        color: #fff;
        text-align: center;
    }

    .habit-card {
        background-color: #1e1e1e;
        border-radius: 8px;
        padding: 1.5rem;
        transition: transform 0.2s ease-in-out;
        border: 1px solid #333;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        width: 25%;
    }

    .habit-card:hover {
        transform: translateY(-1px);
    }

    .habit-header {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        margin-bottom: 1rem;
    }

    .habit-header h3 {
        margin: 0;
        font-size: 1.5rem;
        color: #fff;
    }

    .habit-actions {
        display: flex;
        justify-content: space-between;
        margin-top: 1.5rem;
    }

    .edit-btn, .remove-btn {
        padding: 0.5rem 1rem;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        font-size: 0.9rem;
        transition: background-color 0.2s;
        flex: 1;
        margin: 0 0.5rem;
    }

    .edit-btn {
        background-color: #0061f2;
    }

    .edit-btn:hover {
        background-color: #0051d1;
    }

    .remove-btn {
        background-color: #dc3545;
    }

    .remove-btn:hover {
        background-color: #bb2d3b;
    }

    .description {
        color: #aaa;
        margin-bottom: 1.5rem;
        font-size: 0.9rem;
        height: 3rem;
    }

    .stats {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        gap: 1rem;
        margin-bottom: 1.5rem;
    }

    .stat {
        display: flex;
        flex-direction: column;
        align-items: flex-start;
    }

    .label {
        font-size: 0.8rem;
        color: #888;
    }

    .value {
        font-size: 1.1rem;
        color: #fff;
        font-weight: bold;
    }

    .completions {
        border-top: 1px solid #333;
        padding-top: 1rem;
    }

    .completions h4 {
        margin: 0 0 1rem 0;
        color: #fff;
    }

    .completion-list {
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
    }

    .completion-item {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        font-size: 0.9rem;
    }

    .completion-date {
        color: #888;
        min-width: 100px;
    }

    .completion-status {
        width: 24px;
        height: 24px;
        display: flex;
        align-items: center;
        justify-content: center;
        border-radius: 50%;
        background-color: #dc3545;
        color: white;
    }

    .completion-status.completed {
        background-color: #198754;
    }

    .completion-notes {
        color: #aaa;
        font-style: italic;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
    }

    .loading {
        text-align: center;
        color: #888;
        font-size: 1.2rem;
        margin: 2rem 0;
    }

    .error-message {
        background-color: #dc3545;
        color: white;
        padding: 1rem;
        border-radius: 4px;
        margin-bottom: 2rem;
        text-align: center;
    }

    .no-habits {
        text-align: center;
        color: #888;
        margin: 4rem 0;
    }

    .add-btn {
        display: inline-block;
        background-color: #0061f2;
        color: white;
        padding: 0.75rem 1.5rem;
        border-radius: 4px;
        text-decoration: none;
        margin-top: 1rem;
        transition: background-color 0.2s;
    }

    .add-btn:hover {
        background-color: #0051d1;
    }

    @media (max-width: 768px) {
        .habits-grid {
            grid-template-columns: 1fr;
        }

        .stats {
            grid-template-columns: 1fr;
        }
    }
</style>