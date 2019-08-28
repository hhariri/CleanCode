package com.jetbrains.rider.settings

import com.jetbrains.rider.settings.simple.SimpleOptionsPage

class CleanCodeOptionsPage : SimpleOptionsPage("CleanCode", "MO.CleanCode") {
    
    override fun getId(): String {
        return "MO.CleanCode";
    }
}