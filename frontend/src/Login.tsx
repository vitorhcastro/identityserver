import React, { useEffect } from 'react';
import { useAuth } from './auth';

const Login: React.FC = () => {
    const { isAuthenticated, signIn } = useAuth();

    useEffect(() => {
        signIn();
    }, [isAuthenticated, signIn]);


    return <div>Redirecting to login...</div>;
};

export default Login;
