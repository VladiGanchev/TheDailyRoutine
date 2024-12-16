import { writable } from "svelte/store";


export const user = writable(null);

// Function to log in
export const login = (userData) => {
	user.set(userData);
	localStorage.setItem("user", JSON.stringify(userData));
};

// Function to log out
export const logout = () => {
	user.set(null);
	localStorage.removeItem("user");
};

// Initialize the store with user data from localStorage (if exists)
if (localStorage.getItem("user")) {
	user.set(JSON.parse(localStorage.getItem("user")));
}