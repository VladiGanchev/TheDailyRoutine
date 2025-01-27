<script>
  import { onMount } from "svelte";
  import { fetchGet } from "../js/fetch.js";

  let users = [];
  let loading = true;
  let error = null;

  async function fetchJSON(url) {
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }
    return response.json();
  }

  async function fetchUsersAndHabits() {
    try {
      const usersResponse = await fetchJSON(
        "http://localhost:7155/api/authAPI/all"
      );

      if (usersResponse.success) {
        const usersWithHabits = await Promise.all(
          usersResponse.data.map(async (user) => {
            try {
              const habitsResponse = await fetchGet(
                `/api/habits/${user.id}/public`
              );
              return {
                ...user,
                habits: habitsResponse.data || [],
              };
            } catch (err) {
              console.error(`Failed to fetch habits for user ${user.id}:`, err);
              return { ...user, habits: [] }; // Fallback for failed habits fetch
            }
          })
        );
        users = usersWithHabits;
      } else {
        error = "Failed to fetch users.";
      }
    } catch (err) {
      console.error("Error fetching data:", err);
      error = "An error occurred while fetching data.";
    } finally {
      loading = false;
    }
  }

  onMount(() => {
    fetchUsersAndHabits();
  });
</script>

<div>
  {#if loading}
    <p>Loading...</p>
  {:else if error}
    <p>{error}</p>
  {:else}
    <table>
      <thead>
        <tr>
          <th>User</th>
          <th>Habit</th>
          <th>Current Streak</th>
          <th>Best Streak</th>
        </tr>
      </thead>
      <tbody>
        {#each users as user}
          <tr class="user-row">
            <td>{user.username}</td>
            <td colspan="3">
              {#if user.habits.length === 0}
                <span class="no-habits">No public habits available</span>
              {/if}
            </td>
          </tr>
          {#each user.habits as habit}
            <tr class="habit-row">
              <td></td>
              <td>{habit.title}</td>
              <td>{habit.currentStreak}</td>
              <td>{habit.bestStreak}</td>
            </tr>
          {/each}
        {/each}
      </tbody>
    </table>
  {/if}
</div>

<style>
  table {
    width: 100%;
    border-collapse: collapse;
    margin-top: 1rem;
  }

  th,
  td {
    border: 1px solid #1b1c3b;
    text-align: left;
    padding: 0.75rem;
  }

  th {
    background-color: #1a102f;
  }

  .user-row {
    background-color: #1f193d;
    font-weight: bold;
  }

  .habit-row {
    background-color: #1e163e;
  }

  .no-habits {
    color: #888;
    font-style: italic;
  }
</style>
