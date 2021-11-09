import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

import { AuthService } from "./auth.service";

@Component({
    selector: "app-login-callback",
    templateUrl: "./login-callback.component.html",
    styleUrls: ["./login-callback.component.scss"]
})
export class LoginCallbackComponent implements OnInit {

    constructor(private readonly auth: AuthService, private readonly router: Router) {}

    async ngOnInit() {
        const result = await this.auth.loginCallback();
        await this.router.navigateByUrl(result.returnUrl, { replaceUrl: true });
    }
}