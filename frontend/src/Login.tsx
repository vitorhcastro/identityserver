import React from 'react';
import userManager from './userManager';

const Login: React.FC = () => {
    const handleLogin = () => {
        userManager.signinRedirect();
    };

    return (
        <button onClick={handleLogin}>Login</button>
    );
};

export default Login;
