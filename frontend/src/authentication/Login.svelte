<script>
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
                navigateTo("#/today");
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

<div class="auth-container">
    <form on:submit|preventDefault={handleLogin} class="auth-form">
        <h2>Login</h2>
        
        {#if errorMessage}
            <div class="error-message">{errorMessage}</div>
        {/if}

        <div class="input-group">
            <label for="identifier">identifier</label>
            <input
                type="identifier"
                id="identifier"
                bind:value={identifier}
                placeholder="Enter your identifier"
                required
            />
        </div>

        <div class="input-group">
            <label for="password">Password</label>
            <div class="password-input">
                <input
                    type={showPassword ? 'text' : 'password'}
                    id="password"
                    bind:value={password}
                    placeholder="Enter your password"
                    required
                />
                <button
                    type="button"
                    class="toggle-password"
                    on:click={togglePasswordVisibility}
                >
                    {showPassword ? '👁️‍🗨️' : '👁️'}
                </button>
            </div>
        </div>

        <button type="submit" class="submit-button" disabled={isLoading}>
            {isLoading ? 'Logging in...' : 'Log In'}
        </button>

        <div class="auth-links">
			<a href="/#/register">Don't have an account? Register</a>
        </div>
    </form>
</div>

<style>
    .auth-container {
        display: flex;
        justify-content: center;
        align-items: center;
        min-height: calc(100vh - 100px);
        padding: 1rem;
    }

    .auth-form {
        background: #1a1a1a;
        padding: 2rem;
        border-radius: 8px;
        width: 100%;
        max-width: 400px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    }

    h2 {
        text-align: center;
        margin-bottom: 2rem;
        color: #fff;
    }

    .input-group {
        margin-bottom: 1.5rem;
    }

    .input-group label {
        display: block;
        margin-bottom: 0.5rem;
        color: #b0b0b0;
    }

    input {
        width: 100%;
        padding: 0.75rem;
        border: 1px solid #333;
        border-radius: 4px;
        background: #2a2a2a;
        color: #fff;
    }

    input:focus {
        outline: none;
        border-color: #3b82f6;
        box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.2);
    }

    .password-input {
        position: relative;
    }

    .toggle-password {
        position: absolute;
        right: 0.75rem;
        top: 50%;
        transform: translateY(-50%);
        background: none;
        border: none;
        cursor: pointer;
        padding: 0;
        font-size: 1.25rem;
    }

    .submit-button {
        width: 100%;
        padding: 0.75rem;
        background: #3b82f6;
        color: white;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        font-size: 1rem;
        transition: background-color 0.2s;
    }

    .submit-button:hover:not(:disabled) {
        background: #2563eb;
    }

    .submit-button:disabled {
        opacity: 0.7;
        cursor: not-allowed;
    }

    .error-message {
        background: #dc2626;
        color: white;
        padding: 0.75rem;
        border-radius: 4px;
        margin-bottom: 1rem;
    }

    .auth-links {
        margin-top: 1rem;
        text-align: center;
    }

    .auth-links a {
        color: #3b82f6;
        text-decoration: none;
    }

    .auth-links a:hover {
        text-decoration: underline;
    }
</style>