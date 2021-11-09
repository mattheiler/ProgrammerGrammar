import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { RegisterService } from "./register.service";
import { RegisterRequest } from "./register-request";

@Component({
    selector: "app-register",
    templateUrl: "./register.component.html",
    styleUrls: ["./register.component.scss"]
})
export class RegisterComponent {

    constructor(private readonly registration: RegisterService, private readonly router: Router) {}

    form = new RegisterRequest();

    register() {
        this.registration.register(this.form).subscribe(() => {
            this.router.navigate(["/"]);
        });
    }

}