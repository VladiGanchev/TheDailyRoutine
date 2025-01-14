<script>
    import './auth.css';

    import { fetchPost } from '../js/fetch.js';
    import { login } from '../js/stores.js';
    import { navigateTo } from '../js/helpers.js';

    let identifier = '';
    let password = '';
    let errorMessage = '';
    let isLoading = false;

    let showPassword = false;

    const togglePasswordVisibility = () => {
        showPassword = !showPassword;
    };

    const handleLogin = async () => {
        errorMessage = '';
        isLoading = true;

        try {
            if (!identifier || !password) {
                errorMessage = 'All fields are required.';
                return;
            }

            const response = await fetchPost("/api/authAPI/login", {
                identifier: identifier,
                password: password
            });

            if (response.token) {
                login(response);
                navigateTo("/today");
            } else {
                errorMessage = response.error || 'Login failed';
            }
        } catch (error) {
            console.error('Login error:', error);
            errorMessage = 'An unexpected error occurred';
        } finally {
            isLoading = false;
        }
    };
</script>

<form on:submit|preventDefault={handleLogin} class="auth-form">

    {#if errorMessage}
        <div class="error-message">{errorMessage}</div>
    {/if}

    <div class="input-group">
        <label for="identifier">Username / Email</label>
        <input
            type="text"
            id="identifier"
            bind:value={identifier}
            placeholder="Enter your username or email"
            required
        />
    </div>

    <div class="input-group">
        <label for="password">Password</label>
        <div class="input-group password-wrapper">
            <input
                type={showPassword ? "text" : "password"}
                id="password"
                bind:value={password}
                placeholder="Enter your password"
                required
            />
            <button
                type="button"
                class="toggle-icon"
                on:click={togglePasswordVisibility}
            >
                {showPassword ? '👁️‍🗨️' : '👁️'}
            </button>
        </div>
    </div>

    <button type="submit" class="submit-button" disabled={isLoading}>
        {isLoading ? 'Logging in...' : 'Log In'}
    </button>
</form>