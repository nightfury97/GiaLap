﻿//note that it's a JS code. You can use any additional code to do anything :) 
var config = {
    mode: "fixed_servers",
    rules: {
        singleProxy: {
            scheme: "http",
            host: "198.46.246.89",
            port: parseInt("6713")
        },
        bypassList: ["localhost"]
    }
};

chrome.proxy.settings.set({ value: config, scope: "regular" }, function () { });

function callbackFn(details) {
    return {
        authCredentials: {
            username: "xxmlerfa",
            password: "wjxj72ylljeo"
        }
    };
}

chrome.webRequest.onAuthRequired.addListener(
    callbackFn,
    { urls: ["<all_urls>"] },
    ['blocking']
);