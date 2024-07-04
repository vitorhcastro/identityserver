import React from 'react';
import './Home.css';

const Home: React.FC = () => {
    return (
        <div className="home-container">
            <h1>Welcome to the IdentityServer Management Portal</h1>
            <p>Use the navigation bar to manage clients, API resources, and API scopes.</p>
        </div>
    );
};

export default Home;
