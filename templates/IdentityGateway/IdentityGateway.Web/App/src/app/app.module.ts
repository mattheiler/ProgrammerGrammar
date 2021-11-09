import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";

import { SharedModule } from "./shared";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { AuthModule } from "./auth";
import { ForbiddenComponent } from "./forbidden/forbidden.component";
import { UnauthorizedComponent } from "./unauthorized/unauthorized.component";

@NgModule({
    declarations: [
        AppComponent,
        ForbiddenComponent,
        UnauthorizedComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        FormsModule,
        HttpClientModule,
        SharedModule,
        AppRoutingModule,
        AuthModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {
}