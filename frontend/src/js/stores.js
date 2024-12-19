// import { writable } from "svelte/store";


// export const user = writable(null);
// // Function to log in
// export const login = (userData) => {
// 	user.set(userData);
// 	localStorage.setItem("user", JSON.stringify(userData));

// };

// // Function to log out
// export const logout = () => {
// 	user.set(null);
// 	localStorage.removeItem("user");
// };

// // Initialize the store with user data from localStorage (if exists)
// if (localStorage.getItem("user")) {
// 	user.set(JSON.parse(localStorage.getItem("user")));
// }
import { writable } from "svelte/store";

export const user = writable(null);
export const habits = writable([]);

// Function to log in
export const login = (userData) => {
	user.set(userData);
	localStorage.setItem("user", JSON.stringify(userData));

	// Load habits for the user
	const userHabits = JSON.parse(localStorage.getItem(`habits_${userData.id}`)) || [];
	habits.set(userHabits);
};

// Function to log out
export const logout = () => {
	user.set(null);
	habits.set([]);
	localStorage.removeItem("user");
};

// Initialize the store with user data from localStorage (if exists)
if (localStorage.getItem("user")) {
	const userData = JSON.parse(localStorage.getItem("user"));
	user.set(userData);

	// Load habits for the user
	const userHabits = JSON.parse(localStorage.getItem(`habits_${userData.id}`)) || [];
	habits.set(userHabits);
}

// Function to add a habit
export const addHabit = (habit) => {
	habits.update((currentHabits) => {
		const updatedHabits = [...currentHabits, habit];
		const userData = JSON.parse(localStorage.getItem("user"));
		if (userData) {
			localStorage.setItem(`habits_${userData.id}`, JSON.stringify(updatedHabits));
		}
		return updatedHabits;
	});
};

// Function to delete a habit
export const deleteHabit = (index) => {
	habits.update((currentHabits) => {
		const updatedHabits = currentHabits.filter((_, i) => i !== index);
		const userData = JSON.parse(localStorage.getItem("user"));
		if (userData) {
			localStorage.setItem(`habits_${userData.id}`, JSON.stringify(updatedHabits));
		}
		return updatedHabits;
	});
};
