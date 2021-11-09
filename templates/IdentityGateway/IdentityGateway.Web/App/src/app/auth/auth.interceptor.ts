import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { Observable } from "rxjs";
import { mergeMap } from "rxjs/operators";
import { startsWith } from "lodash-es";

import { AuthService } from "./auth.service";

function isSameOrigin(req: HttpRequest<unknown>) {

    // It's an absolute url with the same origin.
    if (startsWith(req.url, `${location.origin}/`)) {
        return true;
    }

    // It's a protocol relative url with the same origin.
    // For example: //www.example.com/api/Products
    if (startsWith(req.url, `//${location.host}/`)) {
        return true;
    }

    // It's a relative url like /api/Products
    if (/^\/[^\/].*/.test(req.url)) {
        return true;
    }

    // It's an absolute or protocol relative url that doesn't have the same origin.
    return false;
}

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor(private readonly auth: AuthService) {}

    intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

        // use this in the auth service to prevent deadlocks
        if (request.withCredentials === false)
            return next.handle(request);

        return this.auth.user$.pipe(mergeMap(user => {
            if (user != null && isSameOrigin(request)) {
                request = request.clone({
                    withCredentials: true,
                    setHeaders: {
                        Authorization: `${user.token_type} ${user.access_token}`
                    }
                });
            }
            return next.handle(request);
        }));
    }
}