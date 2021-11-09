import { NgModule } from "@angular/core";

import { SharedModule } from "../shared";

import { UsersRoutingModule } from "./users-routing.module";
import { UsersComponent } from "./users.component";
import { UserComponent } from "./user.component";

@NgModule({
    declarations: [
        UsersComponent,
        UserComponent
    ],
    imports: [
        SharedModule,
        UsersRoutingModule
    ]
})
export class UsersModule {
}