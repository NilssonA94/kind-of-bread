export interface Bread {
    id?: number;
    name: string;
    grain: string;
    fermented: boolean;
}

export interface LiquidBread extends Bread {
    viscosity: string;
}


export interface SolidBread extends Bread {
    texture: string;
}


export interface Sandwich extends SolidBread {
    dressed: boolean;
}

export interface SimpleSandwich extends Sandwich {
    toppings: string[];
}

export interface DoubleSandwich extends Sandwich {
    fillings: string[];
}

export interface MultiSandwich extends Sandwich {
    toppings: string[];
    fillings: string[];
}

export interface Boatlike extends SolidBread {
    fillings: string[];
}

export interface Extruded extends SolidBread {
    shape: string;
}