import { Injectable } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { first } from "rxjs/operators";

import { LoginRequest } from "./login-request";
import { LoginError } from "./login-error";

@Injectable({ providedIn: "root" })
export class LoginService {

    constructor(private readonly http: HttpClient, private readonly route: ActivatedRoute) {}

    async login(request: LoginRequest, returnUrl = this.route.snapshot.queryParams["ReturnUrl"] || "/home"): Promise<void> {
        try {
            await this.http.post("/api/accounts/login", request).pipe(first()).toPromise();
            location.href = returnUrl;
        } catch (e) {
            if (e instanceof HttpErrorResponse) {
                let messages: string[];
                switch (e.status) {
                case 400:
                    messages = e.error as string[];
                    break;
                case 500:
                    messages = ["Something unexpected happened."];
                    break;
                default:
                    throw e;
                }
                throw new LoginError({ messages });
            } else throw e;
        }
    }

}