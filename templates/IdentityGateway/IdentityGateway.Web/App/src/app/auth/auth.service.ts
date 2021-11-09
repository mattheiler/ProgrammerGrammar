import { Injectable } from "@angular/core";
import { User, UserManager } from "oidc-client";
import { ReplaySubject } from "rxjs";
import { map, first } from "rxjs/operators";

export interface AuthLoginCallbackResult {
    returnUrl: string;
}

export interface AuthLogoutCallbackResult {
    returnUrl: string;
}

@Injectable({ providedIn: "root" })
export class AuthService {

    private userManager!: UserManager;

    private userSubject = new ReplaySubject<User | null>(1);

    readonly user$ = this.userSubject.asObservable();

    readonly userIsAuthenticated$ = this.userSubject.pipe(map(user => user != null));

    async init(): Promise<void> {

        const userManager = new UserManager({
            authority: "http://localhost:59001",
            client_id: "IdentityGateway",
            redirect_uri: location.origin + "/authentication/login-callback",
            post_logout_redirect_uri: location.origin + "/authentication/logout-callback",
            response_type: "code",
            scope: "openid profile"
        });

        this.userManager = userManager;

        this.userSubject.next(await userManager.getUser());

        userManager.events.addUserLoaded(async user => {
            this.userSubject.next(user);
        });
        userManager.events.addUserUnloaded(async () => {
            this.userSubject.next(null);
        });
    }

    getUser(): Promise<User | null> {
        return this.user$.pipe(first()).toPromise();
    }

    getUserIsAuthenticated(): Promise<boolean> {
        return this.userIsAuthenticated$.pipe(first()).toPromise();
    }

    getUserIsAuthorized(claim: string): Promise<boolean> {
        return this.getUser().then(user => user?.profile[claim] != null);
    }

    async login(returnUrl = "/") {
        return await this.userManager.signinRedirect({ useReplaceToNavigate: true, data: { returnUrl } });
    }

    async loginCallback(): Promise<AuthLoginCallbackResult> {
        const user = await this.userManager.signinRedirectCallback();
        const { returnUrl } = user.state || {};
        return { returnUrl };
    }

    async logout(returnUrl = "/") {
        await this.userManager.signoutRedirect({ useReplaceToNavigate: true, data: { returnUrl } });
    }

    async logoutCallback(): Promise<AuthLogoutCallbackResult> {
        const response = await this.userManager.signoutRedirectCallback();
        const { returnUrl } = response.state || {};
        return { returnUrl };
    }
}