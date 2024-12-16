const domain = 'http://localhost:8080';

// Fetch get request
export async function fetchGet(relative_url) {

	let url = domain.concat(relative_url);

	const res = await fetch(url, {
		credentials: 'include'
	});
	return await res.json();
}

// Fetch post request with JSON body
export async function fetchPost(relative_url, data) {

	let url = domain.concat(relative_url);

	const res = await fetch(url, {
		method: 'POST',
		mode: 'cors',
		credentials: 'include',
		headers: {
			'Content-Type': 'application/json',
			'Accept': 'application/json'
		},
		body: JSON.stringify(data),
	});

	return await res.json();
}