import { NgModule } from "@angular/core";

import { SharedModule } from "../shared";

import { RolesRoutingModule } from "./roles-routing.module";
import { RolesComponent } from "./roles.component";
import { RoleComponent } from "./role.component";

@NgModule({
    declarations: [
        RolesComponent,
        RoleComponent
    ],
    imports: [
        SharedModule,
        RolesRoutingModule
    ]
})
export class RolesModule {
}