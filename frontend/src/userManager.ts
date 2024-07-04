import { UserManager, WebStorageStateStore } from 'oidc-client';

const settings = {
    authority: 'https://localhost:7196',
    client_id: 'react-client',
    redirect_uri: 'http://localhost:3000/callback',
    response_type: 'code',
    scope: 'openid profile identity-server-api',
    post_logout_redirect_uri: 'http://localhost:3000/',
};

const userManager = new UserManager(settings);

export default userManager;
