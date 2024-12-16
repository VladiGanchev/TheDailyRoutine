<script>
	import { fetchPost } from '../js/fetch.js';
	import { login } from '../js/stores.js';
	import { navigateTo } from '../js/helpers.js';

	let username = '';
	let password = '';
	let errorMessage = '';

	let showPassword = false;

	const togglePasswordVisibility = () => {
		showPassword = !showPassword;
	};

	const handleLogin = async () => {
		errorMessage = '';

		if (!username || !password) {
			errorMessage = 'All fields are required.';
		} else {
			const body = {
				username: username,
				password: password,
			}
			const response = await fetchPost("/api/auth/login", body)
			if (response.token) {
				login(response);
				navigateTo("/calendar")
			} else {
				errorMessage = response.error;
			}
		}
	};
</script>

<form autocomplete="off" class="login-form" on:submit|preventDefault={handleLogin}>
	{#if errorMessage}
		<div class="error-message">{errorMessage}</div>
	{/if}

	<div class="input-group">
		<label for="username">Username</label>
		<input
			type="text"
			id="username"
			placeholder="Enter your username"
			bind:value={username}
		/>
	</div>

	<div class="input-group">
		<label for="password">Password</label>
		<div class="input-group password-wrapper">
			<input
				type={showPassword ? 'text' : 'password'}
				id="password"
				placeholder="Enter your password"
				autocomplete="new-password"
				bind:value={password}
			/>
			<!-- Toggle Icon -->
			<button
				type="button"
				class="toggle-icon"
				on:click={togglePasswordVisibility}
				aria-label="Toggle Password Visibility"
			>
				{#if showPassword}
					👁️‍🗨️
				{:else}
					👁️
				{/if}
			</button>
	</div>

	</div>

	<button type="submit" class="login-button">Log In</button>
</form>

<style>
    .login-form {
        display: flex;
        flex-direction: column;
        gap: 1rem;
    }

    .input-group {
        display: flex;
        flex-direction: column;
				text-align: left;
    }

    .input-group label {
        margin-bottom: 0.5rem;
        font-weight: bold;
        color: #b0b0b0;
    }

    .input-group input {
        padding: 0.75rem;
        border: 1px solid #444;
        border-radius: 6px;
        background-color: #2e2e2e;
        color: #ffffff;
    }

    .input-group input:focus {
        outline: none;
        border-color: #3b82f6;
        box-shadow: 0 0 8px rgba(59, 130, 246, 0.8);
    }

    .password-wrapper {
        position: relative;
    }

    .toggle-icon {
        position: absolute;
        right: 5px;
				top: 5px;
        background: transparent;
        border: none;
        cursor: pointer;
        font-size: 1.2rem;
        color: #666;
    }

    .toggle-icon:focus {
        outline: none;
    }

    .toggle-icon:hover {
        color: #000;
    }

    .login-button {
        background-color: #2563eb;
        color: #ffffff;
        border: none;
        padding: 0.75rem;
        font-size: 1rem;
        border-radius: 6px;
        cursor: pointer;
        transition: background-color 0.3s ease;
    }

    .login-button:hover {
        background-color: #3b82f6;
    }

    .error-message {
        color: #f87171;
        background-color: #2e2e2e;
        padding: 0.5rem;
        border-radius: 5px;
        text-align: center;
        font-weight: bold;
    }
</style>