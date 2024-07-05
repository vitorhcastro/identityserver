import React, { useState, useEffect, ChangeEvent } from 'react';
import apiClient from './apiClient';

interface ApiScope {
    id?: number;
    name: string;
    displayName: string;
    description?: string;
    enabled: boolean;
}

const ApiScopeManagement: React.FC = () => {
    const [apiScopes, setApiScopes] = useState<ApiScope[]>([]);
    const [apiScope, setApiScope] = useState<ApiScope>({ name: '', displayName: '', description: '', enabled: true });

    useEffect(() => {
        apiClient.get<ApiScope[]>('/api/apiscopes')
            .then(response => setApiScopes(response.data))
            .catch(error => console.error(error));
    }, []);

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setApiScope(prevState => ({ ...prevState, [name]: value }));
    };

    const handleSubmit = () => {
        apiClient.post('/api/apiscopes', apiScope)
            .then(response => {
                setApiScopes([...apiScopes, response.data]);
                setApiScope({ name: '', displayName: '', description: '', enabled: true });
            })
            .catch(error => console.error(error));
    };

    return (
        <div>
            <h2>API Scope Management</h2>
            <input type="text" name="name" value={apiScope.name} onChange={handleChange} placeholder="Name" />
            <input type="text" name="displayName" value={apiScope.displayName} onChange={handleChange} placeholder="Display Name" />
            <input type="text" name="description" value={apiScope.description} onChange={handleChange} placeholder="Description" />
            <input type="checkbox" name="enabled" checked={apiScope.enabled} onChange={e => setApiScope({ ...apiScope, enabled: e.target.checked })} /> Enabled
            <button onClick={handleSubmit}>Add API Scope</button>
            
            <h3>Existing API Scopes</h3>
            <ul>
                {apiScopes.map(as => (
                    <li key={as.name}>{as.displayName}</li>
                ))}
            </ul>
        </div>
    );
};

export default ApiScopeManagement;
