const TOKEN_KEY = 'documentFlowToken';

export function getToken() {
    return localStorage.getItem(TOKEN_KEY);
}

export function setToken(token) {
    localStorage.setItem(TOKEN_KEY, token);
}

export function clearToken() {
    localStorage.removeItem(TOKEN_KEY);
}

async function request(url, options = {}) {
    const token = getToken();

    const response = await fetch(url, {
        ...options,
        headers: {
            'Content-Type': 'application/json',
            ...(token ? { Authorization: `Bearer ${token}` } : {}),
            ...options.headers
        }
    });

    if (response.status === 401) {
        clearToken();
        window.location.href = '/login';
        throw new Error('Не авторизован');
    }

    if (!response.ok) {
        const text = await response.text();
        throw new Error(text || `Ошибка запроса: ${response.status}`);
    }

    if (response.status === 204) {
        return null;
    }

    return response.json();
}

export const api = {
    login: (uid) =>
        request(`/api/Users?UId=${encodeURIComponent(uid)}`),

    getFlows: () =>
        request('/api/DocumentFlow'),

    getFlow: (id) =>
        request(`/api/DocumentFlow/${id}`),

    createFlow: (flow) =>
        request('/api/DocumentFlow', {
            method: 'POST',
            body: JSON.stringify(flow)
        }),

    addDocumentToFlow: (flowId, document) =>
        request(`/api/DocumentFlow/${flowId}`, {
            method: 'POST',
            body: JSON.stringify(document)
        }),

    getDocuments: () =>
        request('/api/Document'),

    getDocument: (id) =>
        request(`/api/Document/${id}`)
};