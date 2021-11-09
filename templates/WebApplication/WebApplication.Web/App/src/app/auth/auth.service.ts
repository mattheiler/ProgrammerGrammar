import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { User, UserManager, UserManagerSettings } from "oidc-client";
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

    constructor(private readonly http: HttpClient) {}

    readonly user$ = this.userSubject.asObservable();

    readonly userIsAuthenticated$ = this.userSubject.pipe(map(user => user != null));

    async init(): Promise<void> {

        const authority = await this.http.get(".authority", { withCredentials: false, responseType: "text" }).pipe(first()).toPromise();
        const settings: UserManagerSettings = {
            authority,
            client_id: "WebApplication",
            redirect_uri: location.origin + "/authentication/login-callback",
            post_logout_redirect_uri: location.origin + "/authentication/logout-callback",
            response_type: "code",
            scope: "openid profile"
        };
        const userManager = new UserManager(settings);
        const user = await userManager.getUser();

        this.userManager = userManager;
        this.userSubject.next(user);

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