import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

import { AuthService } from "./auth.service";

@Component({
    selector: "app-logout-callback",
    templateUrl: "./logout-callback.component.html",
    styleUrls: ["./logout-callback.component.scss"]
})
export class LogoutCallbackComponent implements OnInit {

    constructor(private readonly auth: AuthService, private readonly router: Router) {}

    async ngOnInit() {
        const result = await this.auth.logoutCallback();
        await this.router.navigateByUrl(result.returnUrl, { replaceUrl: true });
    }

}