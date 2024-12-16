<script>
	import { fetchPost } from '../js/fetch.js';
	import { login } from '../js/stores.js';
	import { navigateTo } from '../js/helpers.js';

	let email = '';
	let username = '';
	let password = '';
	let confirmPassword = '';
	let errorMessage = '';

	let showPassword = false;
	let showConfirmPassword = false;

	const togglePasswordVisibility = () => {
		showPassword = !showPassword;
	};

	const toggleConfirmPasswordVisibility = () => {
		showConfirmPassword = !showConfirmPassword;
	}

	const isValidEmail = email => {
		// Regular expression for validating email
		const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
		return emailRegex.test(email);
	};

	const handleRegister = async () => {
		// Reset error message
		errorMessage = '';

		// Validation Logic
		if (!email || !username || !password || !confirmPassword) {
			errorMessage = 'All fields are required.';
		} else if (password !== confirmPassword) {
			errorMessage = 'Passwords do not match.';
		} else if (!isValidEmail(email)) {
			errorMessage = 'Please enter a valid email address.';
		} else {
			const body = {
				username: username,
				password: password,
			}
			const response = await fetchPost("/api/auth/register", body)
			if (response.status === 201) {
				login(response);
				navigateTo("/calendar")
			} else {
				errorMessage = response.statusText;
			}
		}
	};
</script>

<form autocomplete="off" class="register-form" on:submit|preventDefault={handleRegister}>
	{#if errorMessage}
		<div class="error-message">{errorMessage}</div>
	{/if}

	<div class="input-group">
		<label for="email">Email</label>
		<input
			type="email"
			id="email"
			placeholder="Enter your email"
			bind:value={email}
		/>
	</div>

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

	<div class="input-group">
		<label for="confirmPassword">Confirm Password</label>
		<div class="input-group password-wrapper">
			<input
				type={showConfirmPassword ? 'text' : 'password'}
				id="confirmPassword"
				placeholder="Confirm your password"
				autocomplete="new-password"
				bind:value={confirmPassword}
			/>
			<!-- Toggle Icon -->
			<button
				type="button"
				class="toggle-icon"
				on:click={toggleConfirmPasswordVisibility}
				aria-label="Toggle Confirm Password Visibility"
			>
				{#if showConfirmPassword}
					👁️‍🗨️
				{:else}
					👁️
				{/if}
			</button>
		</div>
	</div>

	<button type="submit" class="register-button">Register</button>
</form>

<style>
    .register-form {
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
        color: #b3b3b3;
    }

    .input-group input {
        padding: 0.75rem;
        border: 1px solid #444;
        border-radius: 5px;
        background-color: #2e2e2e;
        color: #fff;
    }

    .input-group input:focus {
        outline: none;
        border-color: #60a5fa;
        box-shadow: 0 0 8px rgba(96, 165, 250, 0.6);
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

    .register-button {
        padding: 0.75rem;
        background-color: #60a5fa;
        color: #fff;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        font-size: 1rem;
    }

    .register-button:hover {
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
