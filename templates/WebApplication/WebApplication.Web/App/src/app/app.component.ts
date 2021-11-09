import { Component, AfterViewInit } from "@angular/core";
import { BreakpointObserver } from "@angular/cdk/layout";
import { Router, ActivatedRoute, NavigationEnd } from "@angular/router";
import { map, mergeMap, filter } from "rxjs/operators";

import { AuthService } from "./auth";

@Component({
    selector: "app-root",
    templateUrl: "./app.component.html",
    styleUrls: ["./app.component.scss"]
})
export class AppComponent implements AfterViewInit {

    constructor(
        private readonly breakpoints: BreakpointObserver,
        private readonly auth: AuthService,
        private readonly router: Router,
        private readonly route: ActivatedRoute
    ) {
    }

    authenticated$ = this.auth.userIsAuthenticated$;

    login(): void {
        this.auth.login();
    }

    logout(): void {
        this.auth.logout();
    }

    // If any of the activated routes associated with the root (first children)
    // in the primary outlet have the data attribute "hasLayout" set to "true", use the layout.
    // We do this so that we can have blank screen, if needed. Opt-in to the layout, so that those screens don't blink. 
    hasLayout$ = this.router.events.pipe(
        filter(event => event instanceof NavigationEnd),
        map(() => this.route.firstChild as ActivatedRoute),
        filter((route: ActivatedRoute) => route.outlet === "primary"),
        mergeMap((route: ActivatedRoute) => route.data),
        map(({ hasLayout }) => hasLayout === true));

    navOffset = 0;

    navOpened = false;

    navToolbarIsVisible = false;

    navClose(): void {
        this.navOpened = false;
    }

    navOpenedToggle(): void {
        this.navOpened = !this.navOpened;
    }

    // hooks

    ngAfterViewInit(): void {
        this.breakpoints.observe(["(min-width: 1162px)"]).subscribe(result => {
            setTimeout(() => {
                if (result.matches) {
                    this.navOffset = 64;
                    this.navOpened = true;
                    this.navToolbarIsVisible = false;
                } else {
                    this.navOffset = 0;
                    this.navOpened = false;
                    this.navToolbarIsVisible = true;
                }
            });
        });
    }

}