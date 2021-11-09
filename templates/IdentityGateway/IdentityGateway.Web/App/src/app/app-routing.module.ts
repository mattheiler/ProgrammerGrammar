import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { AuthGuard } from "./auth";

import { UnauthorizedComponent } from "./unauthorized/unauthorized.component";
import { ForbiddenComponent } from "./forbidden/forbidden.component";

const routes: Routes = [
    {
        path: "",
        pathMatch: "full",
        redirectTo: "home"
    },
    {
        path: "login",
        loadChildren: () => import("./login").then(module => module.LoginModule)
    },
    {
        path: "register",
        loadChildren: () => import("./register").then(module => module.RegisterModule)
    },
    {
        path: "home",
        loadChildren: () => import("./home").then(module => module.HomeModule),
        canLoad: [AuthGuard],
        data: { hasLayout: true }
    },
    {
        path: "users",
        loadChildren: () => import("./users").then(module => module.UsersModule),
        canLoad: [AuthGuard],
        data: { hasLayout: true }
    },
    {
        path: "roles",
        loadChildren: () => import("./roles").then(module => module.RolesModule),
        canLoad: [AuthGuard],
        data: { hasLayout: true }
    },
    {
        path: "forbidden",
        component: ForbiddenComponent,
        canActivate: [AuthGuard]
    },
    {
        path: "unauthorized",
        component: UnauthorizedComponent
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, { paramsInheritanceStrategy: "always" })],
    exports: [RouterModule]
})
export class AppRoutingModule {
}