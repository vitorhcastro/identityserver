import React from 'react';
import { useAuth } from './auth';

const Login: React.FC = () => {
    const { signIn } = useAuth();

    return (
        <div>
            <h2>Login</h2>
            <button onClick={signIn}>Sign In</button>
        </div>
    );
};

export default Login;
