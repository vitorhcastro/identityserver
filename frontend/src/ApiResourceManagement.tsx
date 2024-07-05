import React, { useState, useEffect, ChangeEvent } from 'react';
import apiClient from './apiClient';

interface ApiResource {
    id?: number;
    name: string;
    displayName: string;
    description?: string;
    enabled: boolean;
}

const ApiResourceManagement: React.FC = () => {
    const [apiResources, setApiResources] = useState<ApiResource[]>([]);
    const [apiResource, setApiResource] = useState<ApiResource>({ name: '', displayName: '', description: '', enabled: true });

    useEffect(() => {
        apiClient.get<ApiResource[]>('/api/apiresources')
        .then(response => setApiResources(response.data))
            .catch(error => console.error(error));
    }, []);

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setApiResource(prevState => ({ ...prevState, [name]: value }));
    };

    const handleSubmit = () => {
        apiClient.post('/api/apiresources', apiResource)
            .then(response => {
                setApiResources([...apiResources, response.data]);
                setApiResource({ name: '', displayName: '', description: '', enabled: true });
            })
            .catch(error => console.error(error));
    };

    return (
        <div>
            <h2>API Resource Management</h2>
            <input type="text" name="name" value={apiResource.name} onChange={handleChange} placeholder="Name" />
            <input type="text" name="displayName" value={apiResource.displayName} onChange={handleChange} placeholder="Display Name" />
            <input type="text" name="description" value={apiResource.description} onChange={handleChange} placeholder="Description" />
            <input type="checkbox" name="enabled" checked={apiResource.enabled} onChange={e => setApiResource({ ...apiResource, enabled: e.target.checked })} /> Enabled
            <button onClick={handleSubmit}>Add API Resource</button>
            
            <h3>Existing API Resources</h3>
            <ul>
                {apiResources.map(ar => (
                    <li key={ar.name}>{ar.displayName}</li>
                ))}
            </ul>
        </div>
    );
};

export default ApiResourceManagement;
