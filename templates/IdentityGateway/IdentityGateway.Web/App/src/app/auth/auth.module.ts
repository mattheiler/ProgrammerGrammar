import { NgModule, APP_INITIALIZER } from "@angular/core";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { RouterModule } from "@angular/router";

import { AuthInterceptor } from "./auth.interceptor";
import { AuthService } from "./auth.service";

import { LoginCallbackComponent } from "./login-callback.component";
import { LogoutCallbackComponent } from "./logout-callback.component";


@NgModule({
    declarations: [LoginCallbackComponent, LogoutCallbackComponent],
    imports: [
        RouterModule.forChild([
            {
                path: "authentication/login-callback",
                component: LoginCallbackComponent
            },
            {
                path: "authentication/logout-callback",
                component: LogoutCallbackComponent
            }
        ])
    ],
    exports: [RouterModule],
    providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthInterceptor,
            multi: true
        },
        {
            provide: APP_INITIALIZER,
            useFactory: (auth: AuthService) => async () => await auth.init(),
            deps: [AuthService],
            multi: true
        }
    ]
})
export class AuthModule {
}