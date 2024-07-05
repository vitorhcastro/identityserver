import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import ClientManagement from './ClientManagement';
import ApiResourceManagement from './ApiResourceManagement';
import ApiScopeManagement from './ApiScopeManagement';
import Login from './Login';
import Home from './Home';
import Callback from './Callback';
import PrivateRoute from './PrivateRoute';
import { AuthProvider, useAuth } from './auth';
import './App.css';

const Navigation: React.FC = () => {
    const { isAuthenticated, signIn, signOut } = useAuth();

    return (
        <nav>
            <ul>
                <li>
                    <Link to="/">Home</Link>
                </li>
                <li>
                    <Link to="/clients">Client Management</Link>
                </li>
                <li>
                    <Link to="/apiresources">API Resource Management</Link>
                </li>
                <li>
                    <Link to="/apiscopes">API Scope Management</Link>
                </li>
                {isAuthenticated ? (
                    <li>
                        <button onClick={signOut}>Logout</button>
                    </li>
                ) : (
                    <li>
                        <button onClick={signIn}>Login</button>
                    </li>
                )}
            </ul>
        </nav>
    );
};

const App: React.FC = () => {
    return (
        <AuthProvider>
            <Router>
                <div className="app-container">
                    <Navigation />
                    <Routes>
                        <Route path="/" element={<Home />} />
                        <Route path="/clients" element={<PrivateRoute><ClientManagement /></PrivateRoute>} />
                        <Route path="/apiresources" element={<PrivateRoute><ApiResourceManagement /></PrivateRoute>} />
                        <Route path="/apiscopes" element={<PrivateRoute><ApiScopeManagement /></PrivateRoute>} />
                        <Route path="/login" element={<Login />} />
                        <Route path="/callback" element={<Callback />} />
                    </Routes>
                </div>
            </Router>
        </AuthProvider>
    );
};

export default App;
