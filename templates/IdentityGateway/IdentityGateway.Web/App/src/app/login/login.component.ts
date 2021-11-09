import { Component, ViewChild, TemplateRef } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";

import { LoginService } from "./login.service";
import { LoginRequest } from "./login-request";
import { LoginError } from "./login-error";

@Component({
    selector: "app-login",
    templateUrl: "./login.component.html",
    styleUrls: ["./login.component.scss"]
})
export class LoginComponent {

    constructor(private readonly logins: LoginService, private readonly dialog: MatDialog) {}

    form = new LoginRequest();

    @ViewChild("errors")
    errorsTemplateRef!: TemplateRef<unknown>;

    async login() {
        try {
            await this.logins.login(this.form);
        } catch (e) {
            if (e instanceof LoginError)
                this.dialog.open(this.errorsTemplateRef, { data: e.messages });
            else throw e;
        }
    }

}