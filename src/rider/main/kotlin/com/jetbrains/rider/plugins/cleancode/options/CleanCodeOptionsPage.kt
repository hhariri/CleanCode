package com.jetbrains.rider.plugins.cleancode.options

import com.jetbrains.rider.settings.simple.SimpleOptionsPage

class CleanCodeOptionsPage : SimpleOptionsPage("CleanCode", "CleanCodeAnalysisOptionsPage") {
    override fun getId(): String {
        return "CleanCodeAnalysisOptionsPage";
    }
}