import { Injectable } from "@angular/core";
import { CanActivate, CanLoad } from "@angular/router";
import { Observable } from "rxjs";
import { mergeMap } from "rxjs/operators";

import { AuthService } from "./auth.service";

@Injectable({
    providedIn: "root"
})
export class AuthGuard implements CanActivate, CanLoad {

    constructor(private readonly auth: AuthService) {}

    canActivate(): Observable<boolean> {
        return this.check();
    }

    canLoad(): Observable<boolean> {
        return this.check();
    }

    private check() {
        return this.auth.userIsAuthenticated$.pipe(mergeMap(async authenticated => {
            if (authenticated)
                return true;
            await this.auth.login();
            return false;
        }));
    }
}