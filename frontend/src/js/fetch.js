const domain = 'http://localhost:7155';

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
    
    try {
        const res = await fetch(url, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(data),
            redirect: 'error' // prevent automatic redirects
        });

        // Check if response is JSON
        const contentType = res.headers.get("content-type");
        if (!contentType || !contentType.includes("application/json")) {
            throw new Error("Response was not JSON");
        }

        const jsonResponse = await res.json();
        
        if (!res.ok) {
            throw new Error(jsonResponse.error || 'Server error');
        }

        return jsonResponse;
    } catch (error) {
        console.error('Fetch error:', error);
        return { error: error.message || 'Failed to connect to the server' };
    }
}

export async function fetchDelete(relative_url) {
	let url = domain.concat(relative_url);
	const res = await fetch(url, {
		method: 'DELETE',
		credentials: 'include'
	});
	return await res.json();
}