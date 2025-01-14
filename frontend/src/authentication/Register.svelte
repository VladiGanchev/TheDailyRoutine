<script>
  	import './auth.css';

	import { fetchPost } from '../js/fetch.js';
	import { login } from '../js/stores.js';
	import { navigateTo } from '../js/helpers.js';

	let email = '';
	let username = '';
	let password = '';
	let confirmPassword = '';
	let errorMessage = '';
	let isLoading = false; 

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
		// Reset error message and loading to true
		errorMessage = '';
		isLoading = true; 

		// Validation Logic
		if (!email || !username || !password || !confirmPassword) {
			errorMessage = 'All fields are required.';
		} else if (password !== confirmPassword) {
			errorMessage = 'Passwords do not match.';
		} else if (!isValidEmail(email)) {
			errorMessage = 'Please enter a valid email address.';
		} else {
			const body = {
				email: email,
				username: username,
				password: password,
				confirmPassword: confirmPassword
			}
			try {
				const response = await fetchPost("/api/authAPI/register", body)  // Updated endpoint
				if (response.token) {
					login(response);
					navigateTo("#/today")
				} else {
					errorMessage = response.error || 'Registration failed';
				}
			} catch (error) {
				console.error('Registration error:', error);
				errorMessage = 'An unexpected error occurred';
			}
			finally {
				isLoading = false;
			}
		}
	};
</script>

<form autocomplete="off" class="auth-form" on:submit|preventDefault={handleRegister}>
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

	<button 
    	type="submit"
    	class="submit-button"
    	disabled={isLoading}>
    	{isLoading ? 'Registering...' : 'Register'}
	</button>
</form>