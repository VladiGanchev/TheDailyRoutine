<script>
	import { onMount } from 'svelte';

	import { logout } from '../js/stores.js';
	import { navigateTo } from '../js/helpers.js';

	onMount(() => {
		logout();
		navigateTo('/');
	})
</script>