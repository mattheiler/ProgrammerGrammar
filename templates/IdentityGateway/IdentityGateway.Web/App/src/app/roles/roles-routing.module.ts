import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { RolesComponent } from "./roles.component";
import { RoleComponent } from "./role.component";

const routes: Routes = [
    {
        path: "",
        component: RolesComponent
    },
    {
        path: ":role",
        component: RoleComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class RolesRoutingModule {
}