<script>
	import Router from "svelte-spa-router";
	import wrap from 'svelte-spa-router/wrap';

	import { user } from './js/stores.js';

	// Route imports
	import Home from "./Home.svelte";
	import MyAccount from "./account/MyAccount.svelte";
	import Calendar from "./calendar/Calendar.svelte";
	import Statistics from "./statistics/Statistics.svelte";
	import MyHabits from "./habits/MyHabits.svelte";
	import CreateHabit from "./habits/CreateHabit.svelte";
	import PickHabits from "./habits/PickHabits.svelte";
	import Logout from './authentication/Logout.svelte';
	import NotFound from "./navigation/NotFound.svelte";
	import { navigateTo } from './js/helpers.js';

	if (!window.location.hash) {
		navigateTo(window.location.pathname);
		window.location.pathname = "";
	}

	let currentUser;
	$: currentUser = $user

	const validateLogin = () => {
		return currentUser.token !== null;
	}

	// Define the route configuration
	const routes = {
		"/": Home,
		"/account": wrap({
			component: MyAccount,
			conditions: [() => {return validateLogin();}]
		}),
		"/calendar": wrap({
			component: Calendar,
			conditions: [() => {return validateLogin();}]
		}),
		"/statistics": wrap({
			component: Statistics,
			conditions: [() => {return validateLogin();}]
		}),
		"/my-habits": wrap({
			component: MyHabits,
			conditions: [() => {return validateLogin();}]
		}),
		"/create-habit": wrap({
			component: CreateHabit,
			conditions: [() => {return validateLogin();}]
		}),
		"/pick-habits": wrap({
			component: PickHabits,
			conditions: [() => {return validateLogin();}]
		}),
		"/logout": wrap({
			component: Logout,
			conditions: [() => {return validateLogin();}]
		}),
		"*": NotFound
	};
</script>

<Router {routes} />